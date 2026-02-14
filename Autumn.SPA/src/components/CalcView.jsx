import { useState } from "react";
import { Calculator, Search } from "lucide-react";
import { api } from "../api";
import TariffPrompt from "./TariffPrompt";

const labelCls = "block text-[11px] font-bold text-fg-dim mb-1 uppercase tracking-[0.05em]";
const inputCls = "w-full px-3 py-2.5 rounded-lg border border-border bg-input-bg text-fg text-sm outline-none font-sans transition-colors duration-150 focus:border-accent";

// Strip commas to get raw number string
const rawNum = (v) => v.replace(/,/g, "");

// Format a number string with commas and 2 decimals
const fmtInput = (v) => {
  const n = parseFloat(rawNum(v));
  if (isNaN(n)) return "";
  return n.toLocaleString("en", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
};

export default function CalcView({ country, countries, setCountry, onSearchCode, initialHscode, initialProduct }) {
  const [calc, setCalc] = useState({
    product: initialProduct || "",
    hscode: initialHscode || "",
    cost: "",
    freight: "",
    insurance: "",
  });
  // Track which money field is focused (show raw value while editing)
  const [focused, setFocused] = useState(null);
  const [calcOut, setCalcOut] = useState(null);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState(null);

  const selCountry = countries.find(c => c.code === country);
  const currency = selCountry?.currency?.split(" ")[0] || "NGN";

  const parseVal = (k) => parseFloat(rawNum(calc[k])) || 0;

  const doCalc = (e) => {
    e.preventDefault();
    if (!country || !calc.hscode) return;
    setBusy(true);
    setError(null);
    api.duty({
      HSCode: calc.hscode,
      Country: country,
      ProductDesc: calc.product,
      Cost: parseVal("cost"),
      Freight: parseVal("freight"),
      Insurance: parseVal("insurance"),
      Currency: currency,
    }).then((resp) => {
      if (resp.success) {
        setCalcOut(resp);
      } else {
        setError(resp.error?.[0] || "Calculation failed");
        setCalcOut(null);
      }
    }).catch((err) => {
      setError(err.message || "Network error");
      setCalcOut(null);
    }).finally(() => setBusy(false));
  };

  const fmt = (n) => currency + " " + Number(n).toLocaleString("en", { minimumFractionDigits: 2, maximumFractionDigits: 2 });

  const canSubmit = country && country === "NG";

  const update = (k, v) => setCalc(prev => ({ ...prev, [k]: v }));

  // Allow only digits, one decimal point, and commas while typing
  const handleMoneyChange = (k, raw) => {
    const cleaned = raw.replace(/[^0-9.,]/g, "");
    update(k, cleaned);
  };

  // On blur: format with commas and 2 decimals, clamp to >= 0
  const handleMoneyBlur = (k) => {
    setFocused(null);
    const n = parseFloat(rawNum(calc[k]));
    if (isNaN(n) || n <= 0) {
      update(k, "");
    } else {
      update(k, fmtInput(calc[k]));
    }
  };

  // On focus: show raw number (no commas) for easy editing
  const handleMoneyFocus = (k) => {
    setFocused(k);
    const n = parseFloat(rawNum(calc[k]));
    if (!isNaN(n) && n > 0) {
      update(k, String(n));
    }
  };

  // Display value: formatted when not focused, raw when focused
  const moneyVal = (k) => focused === k ? calc[k] : (calc[k] ? fmtInput(calc[k]) : "");

  return (
    <div className="max-w-[960px] mx-auto pt-8 px-6 pb-16">
      <h2 className="text-2xl font-extrabold tracking-[-0.02em] mb-1.5 text-fg">Import Duty Calculator</h2>
      <p className="text-fg-sec text-sm mb-6">Estimate total import duties, taxes, and levies payable.</p>

      {!country && <div className="mb-5"><TariffPrompt onSelect={setCountry} countries={countries} /></div>}

      <div className="grid grid-cols-[minmax(260px,340px)_1fr] gap-[18px] items-start">
        <form onSubmit={doCalc} className="p-[22px] rounded-[13px] border border-border bg-surface">
          {country && (
            <div className="flex items-center gap-2 px-3 py-2 rounded-[7px] bg-accent-dim border border-accent-border mb-3.5">
              <span className="text-base">{selCountry?.flag}</span>
              <div>
                <div className="text-xs font-bold text-fg">{selCountry?.name}</div>
                <div className="text-[10px] text-fg-dim">{currency}</div>
              </div>
              <button type="button" onClick={() => setCountry("")} className="ml-auto bg-transparent border-none text-fg-dim cursor-pointer text-[11px] font-sans">Change</button>
            </div>
          )}

          <div className="mb-3.5">
            <label className={labelCls}>Commodity Description</label>
            <input type="text" placeholder="e.g. laptop computer" value={calc.product} onChange={(e) => update("product", e.target.value)} className={inputCls} />
          </div>

          <div className="mb-3.5">
            <div className="flex items-center justify-between mb-1">
              <label className={labelCls + " mb-0"}>HS Code</label>
              {onSearchCode && (
                <button type="button" onClick={() => onSearchCode(calc.product)}
                  className="flex items-center gap-1 bg-transparent border-none text-accent text-[11px] font-semibold cursor-pointer font-sans hover:underline">
                  <Search size={11} /> Find code
                </button>
              )}
            </div>
            <input type="text" placeholder="e.g. 8471.30" value={calc.hscode} onChange={(e) => update("hscode", e.target.value)} className={inputCls} />
          </div>

          <div className="mb-3.5">
            <label className={labelCls}>FOB Cost ({currency})</label>
            <input type="text" inputMode="decimal" placeholder="0.00" value={moneyVal("cost")}
              onChange={(e) => handleMoneyChange("cost", e.target.value)}
              onFocus={() => handleMoneyFocus("cost")}
              onBlur={() => handleMoneyBlur("cost")}
              className={inputCls} />
          </div>

          <div className="mb-3.5">
            <label className={labelCls}>Freight ({currency})</label>
            <input type="text" inputMode="decimal" placeholder="0.00" value={moneyVal("freight")}
              onChange={(e) => handleMoneyChange("freight", e.target.value)}
              onFocus={() => handleMoneyFocus("freight")}
              onBlur={() => handleMoneyBlur("freight")}
              className={inputCls} />
          </div>

          <div className="mb-3.5">
            <label className={labelCls}>Insurance ({currency})</label>
            <input type="text" inputMode="decimal" placeholder="0.00" value={moneyVal("insurance")}
              onChange={(e) => handleMoneyChange("insurance", e.target.value)}
              onFocus={() => handleMoneyFocus("insurance")}
              onBlur={() => handleMoneyBlur("insurance")}
              className={inputCls} />
          </div>

          <button type="submit" disabled={!canSubmit || busy}
            className={`w-full py-[11px] rounded-lg border-none font-bold text-sm font-sans
              ${canSubmit ? 'bg-grad text-btn-text cursor-pointer opacity-100' : 'bg-surface2 text-fg-dim cursor-not-allowed opacity-60'}`}>
            {busy ? "Calculating\u2026" : !country ? "Select a country first" : country !== "NG" ? "Coming soon for this country" : "Calculate Duties"}
          </button>
        </form>

        <div className="p-[22px] rounded-[13px] border border-border bg-surface min-h-[360px]">
          {error && (
            <div className="px-4 py-3 rounded-lg bg-[rgba(224,90,78,0.1)] border border-danger/20 mb-3.5">
              <span className="text-[13px] text-danger">{error}</span>
            </div>
          )}
          {!calcOut && !error ? (
            <div className="text-center py-16 text-fg-dim">
              <Calculator size={36} strokeWidth={1.2} className="mx-auto mb-2 opacity-30" />
              <p className="text-sm">{!country ? "Select a country to get started" : country !== "NG" ? `Duty calculator for ${selCountry?.name || country} coming soon.` : "Fill in the form and click Calculate"}</p>
            </div>
          ) : calcOut && (
            <div className="animate-fade-up">
              <div className="flex items-center gap-2 mb-3.5">
                <span className="text-lg">{selCountry?.flag}</span>
                <div>
                  <div className="text-sm font-bold text-fg">{selCountry?.name} Import Duties</div>
                  <div className="text-[11px] text-fg-dim">HS Code: {calcOut.hsCode}</div>
                </div>
              </div>
              {calcOut.hsCodeDescription && (
                <div className="text-xs text-fg-sec mb-3.5 px-[11px] py-2 rounded-[6px] bg-surface2">
                  {calcOut.hsCodeDescription}
                </div>
              )}
              <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.07em] mb-2.5">CIF Summary ({currency})</div>
              <div className="grid grid-cols-2 gap-[7px] mb-5">
                {[
                  ["Cost", fmt(calcOut.cost)],
                  ["Freight", fmt(calcOut.freight)],
                  ["Insurance", fmt(calcOut.insurance)],
                  ["CIF Total", fmt(calcOut.cif)],
                ].map(([l, v], i) => (
                  <div key={i} className="px-[11px] py-2 rounded-[6px] bg-surface2">
                    <div className="text-[11px] text-fg-dim mb-[1px]">{l}</div>
                    <div className="text-sm font-semibold font-mono text-fg">{v}</div>
                  </div>
                ))}
              </div>
              <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.07em] mb-2.5">Duties, Taxes & Levies ({currency})</div>
              <div className="flex flex-col gap-[5px] mb-5">
                {(calcOut.breakdown || []).map((b, i) => (
                  <div key={i} className="flex justify-between px-[11px] py-2 rounded-[6px] bg-surface2">
                    <span className="text-[13px] text-fg-sec">{b.label} ({b.rate}%)</span>
                    <span className="text-sm font-semibold font-mono text-fg">{fmt(b.amount)}</span>
                  </div>
                ))}
              </div>
              <div className="px-4 py-3.5 rounded-[9px] bg-accent-dim border border-accent-border flex justify-between items-center">
                <span className="text-xs font-bold uppercase text-fg">Total Duties & Taxes</span>
                <span className="text-lg font-extrabold text-accent font-mono">{fmt(calcOut.totalDuty)}</span>
              </div>
              <p className="text-[11px] text-fg-dim mt-2.5 leading-normal italic">Disclaimer: Values are estimates only. Final duties determined by Customs at port of clearance.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
