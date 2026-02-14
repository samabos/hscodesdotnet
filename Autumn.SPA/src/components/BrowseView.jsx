import { useState, useEffect } from "react";
import { ChevronRight, Package } from "lucide-react";
import { api } from "../api";
import { SECTIONS, ICON_MAP } from "../data/sections";
import TariffPrompt from "./TariffPrompt";

const RATE_LABELS = {
  duty: "Import Duty", vat: "VAT", levy: "Levy", sur: "Surcharge",
  etls: "ETL", ciss: "CISS", nac: "NAC", nhil: "NHIL",
  getfund: "GETFund", idf: "IDF", rdf: "RDF",
};
const RATE_FIELDS = ["duty", "vat", "levy", "sur", "etls", "ciss", "nac", "nhil", "getfund", "idf", "rdf"];

function formatTariffEntries(tariff) {
  if (!tariff) return [];
  return RATE_FIELDS
    .filter(f => tariff[f] && tariff[f] !== "0")
    .map(f => [RATE_LABELS[f] || f, `${tariff[f]}%`]);
}

export default function BrowseView({ country, countries, setCountry }) {
  // path entries: { code, description, id, level, childLevel }
  const [path, setPath] = useState([]);
  const [children, setChildren] = useState(null);
  const [leafDetail, setLeafDetail] = useState(null);
  const [loading, setLoading] = useState(false);

  const isRoot = path.length === 0;
  const selCountry = countries.find(c => c.code === country);

  const loadChildren = async (parentId, parentCode, level) => {
    setLoading(true); setChildren(null); setLeafDetail(null);
    try {
      const params = parentId ? { parentId, level } : { parentCode, level };
      const resp = await api.browse(params);
      if (resp.success && resp.records?.length > 0) {
        setChildren(resp.records.sort((a, b) => a.code.localeCompare(b.code)));
      } else {
        setChildren(null);
        // No children — it's a leaf, load tariff
        const code = parentCode || path[path.length - 1]?.code;
        if (code) {
          const noteResp = await api.note(code, country || "NG");
          if (noteResp.success) setLeafDetail(noteResp);
        }
      }
    } catch {
      setChildren(null);
    }
    setLoading(false);
  };

  const navigateSection = (section) => {
    const entry = { code: section.parentCode, description: `Section ${section.code} — ${section.title}`, id: null, level: 1, childLevel: 2 };
    setPath([entry]);
    loadChildren(null, section.parentCode, 2);
  };

  const navigateTo = async (child) => {
    const nextLevel = child.level + 1;
    const entry = { code: child.code, description: child.description, id: child.id, level: child.level, childLevel: nextLevel };
    setPath(prev => [...prev, entry]);
    setLoading(true); setChildren(null); setLeafDetail(null);
    try {
      const resp = await api.browse({ parentId: child.id, level: nextLevel });
      if (resp.success && resp.records?.length > 0) {
        setChildren(resp.records.sort((a, b) => a.code.localeCompare(b.code)));
      } else {
        const noteResp = await api.note(child.code, country || "NG");
        if (noteResp.success) setLeafDetail(noteResp);
      }
    } catch {
      const noteResp = await api.note(child.code, country || "NG").catch(() => null);
      if (noteResp?.success) setLeafDetail(noteResp);
    }
    setLoading(false);
  };

  const browseToLevel = (idx) => {
    if (idx < 0) {
      setPath([]); setChildren(null); setLeafDetail(null);
      return;
    }
    const newPath = path.slice(0, idx + 1);
    setPath(newPath);
    const target = newPath[newPath.length - 1];
    loadChildren(target.id, target.id ? null : target.code, target.childLevel);
  };

  useEffect(() => {
    if (leafDetail && country && path.length > 0) {
      const code = path[path.length - 1].code;
      api.note(code, country).then(resp => { if (resp.success) setLeafDetail(resp); }).catch(() => {});
    }
  }, [country]);

  const leafTariffEntries = leafDetail?.tariff?.[0] ? formatTariffEntries(leafDetail.tariff[0]) : [];

  return (
    <div className="max-w-[960px] mx-auto pt-8 px-6 pb-16">
      <h2 className="text-2xl font-extrabold tracking-[-0.02em] mb-1.5 text-fg">Browse Harmonized System</h2>
      <p className="text-fg-sec text-sm mb-5">Navigate the WCO Harmonized System nomenclature.</p>

      {/* Root grid */}
      {isRoot && !loading && (
        <>
          <div className="grid grid-cols-[repeat(auto-fill,minmax(240px,1fr))] gap-[7px]">
            {SECTIONS.map((s, i) => {
              const Icon = ICON_MAP[s.code] || Package;
              return (
                <button key={i} onClick={() => navigateSection(s)}
                  className="flex items-center gap-3 px-3.5 py-[13px] rounded-[9px] border border-border bg-surface cursor-pointer text-left font-sans transition-all duration-150 hover:border-accent-border hover:bg-surface2">
                  <div className="w-[34px] h-[34px] rounded-lg bg-accent-dim flex items-center justify-center shrink-0">
                    <Icon size={16} className="text-accent" strokeWidth={1.8} />
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="text-xs font-bold text-accent font-mono mb-[1px]">Ch. {s.code}</div>
                    <div className="text-[13px] text-fg leading-snug">{s.title}</div>
                  </div>
                  <ChevronRight size={13} className="text-fg-dim shrink-0" />
                </button>
              );
            })}
          </div>
          <p className="text-xs text-fg-dim mt-4 text-center">
            Try: <strong className="text-accent">Machinery & Electrical Equipment</strong> → Chapter 84 → 8471 → 8471.30
          </p>
        </>
      )}

      {/* Hierarchy tree + children / leaf */}
      {!isRoot && (
        <div className="p-[22px] rounded-[13px] border border-border bg-surface animate-fade-up">
          <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.06em] mb-2.5">Navigation</div>

          {/* "All Sections" root link */}
          <button onClick={() => browseToLevel(-1)}
            className="flex items-center gap-2.5 py-[7px] px-2.5 rounded-[5px] border-none w-full text-left font-sans bg-transparent border-l-[3px] border-l-transparent cursor-pointer">
            <span className="text-[13px] text-accent font-semibold">← All Sections</span>
          </button>

          {/* Path hierarchy rows */}
          <div className="flex flex-col mb-3.5">
            {path.map((entry, idx) => {
              const isLast = idx === path.length - 1;
              const isLeaf = isLast && leafDetail && !children;
              const isClickable = !isLast;
              return (
                <button key={idx} onClick={isClickable ? () => browseToLevel(idx) : undefined}
                  className={`flex items-center gap-2.5 py-[7px] px-2.5 rounded-[5px] border-none w-full text-left font-sans
                    ${isLeaf ? 'bg-accent-dim border-l-[3px] border-l-accent' : isLast ? 'bg-surface2 border-l-[3px] border-l-accent' : 'bg-transparent border-l-[3px] border-l-transparent'}
                    ${isClickable ? 'cursor-pointer' : 'cursor-default'}`}
                  style={{ paddingLeft: `${10 + idx * 22}px` }}>
                  <span className={`font-mono text-[13px] font-semibold min-w-[72px] ${isClickable ? 'text-accent' : 'text-accent'}`}>{entry.code}</span>
                  <span className={`text-[13px] flex-1 ${isLast ? 'text-fg' : 'text-fg-sec'}`}>{entry.description}</span>
                </button>
              );
            })}
          </div>

          {/* Loading */}
          {loading && (
            <div className="text-center py-6">
              <div className="w-[26px] h-[26px] mx-auto mb-2 border-[3px] border-border border-t-accent rounded-full animate-spin-loading" />
              <p className="text-fg-dim text-xs">Loading…</p>
            </div>
          )}

          {/* Child items below the hierarchy */}
          {children && !loading && (
            <div className="flex flex-col gap-[3px]" style={{ paddingLeft: `${Math.min(path.length * 22, 110)}px` }}>
              {children.length === 0 ? (
                <div className="text-xs text-fg-dim py-2">No sub-items found.</div>
              ) : children.map((child, ci) => (
                <button key={ci} onClick={() => navigateTo(child)}
                  className="flex items-center gap-2.5 py-[9px] px-3 rounded-[7px] border border-border bg-surface cursor-pointer text-left font-sans transition-all duration-150 hover:border-accent-border hover:bg-surface2">
                  <span className="font-mono text-[13px] font-bold text-accent min-w-[62px]">{child.code}</span>
                  <span className="text-[13px] text-fg flex-1 leading-snug">{child.description}</span>
                  <ChevronRight size={13} className="text-fg-dim shrink-0" />
                </button>
              ))}
            </div>
          )}

          {/* Leaf detail — tariff */}
          {leafDetail && !loading && (
            <div className="mt-3.5">
              {leafDetail.records?.[0]?.description && (
                <p className="text-sm text-fg leading-relaxed mb-4">{leafDetail.records[0].description}</p>
              )}
              <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.06em] mb-2.5">
                Applicable Tariffs {selCountry && <span className="text-accent">— {selCountry.flag} {selCountry.name}</span>}
              </div>
              {!country ? (
                <TariffPrompt onSelect={setCountry} countries={countries} />
              ) : country !== "NG" ? (
                <div className="text-sm text-fg-dim py-2 italic">Tariff data for {selCountry?.name || country} coming soon.</div>
              ) : leafTariffEntries.length > 0 ? (
                <div className="grid grid-cols-[repeat(auto-fit,minmax(110px,1fr))] gap-2">
                  {leafTariffEntries.map(([l, v], ti) => (
                    <div key={ti} className="px-3.5 py-2.5 rounded-lg bg-surface2 border border-border">
                      <div className="text-[11px] text-fg-dim mb-0.5 uppercase tracking-[0.05em]">{l}</div>
                      <div className="text-base font-bold font-mono text-fg">{v}</div>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="text-sm text-fg-dim py-2">No tariff data available for this code.</div>
              )}
            </div>
          )}
        </div>
      )}
    </div>
  );
}
