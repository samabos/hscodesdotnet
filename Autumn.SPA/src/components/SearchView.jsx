import { useState, useEffect } from "react";
import { ChevronDown, ChevronRight, Calculator } from "lucide-react";
import { api } from "../api";
import SearchBar from "./SearchBar";
import Badge from "./Badge";

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

// Deduplicate: remove results whose code is a parent of another result
function deduplicateResults(flat) {
  const allParentCodes = new Set();
  flat.forEach(r => {
    (r.parentHSCodes || []).forEach(p => allParentCodes.add(p.code));
  });
  return flat.filter(r => {
    const code = r.hsCodes?.[0]?.code || r.code;
    return !allParentCodes.has(code);
  });
}

export default function SearchView({ query, onQueryChange, country, countries, setCountry, onCalc }) {
  const [busy, setBusy] = useState(false);
  const [results, setResults] = useState(null);
  const [exp, setExp] = useState(null);
  const [noteData, setNoteData] = useState({});
  // drill[idx] = { path: [{code,desc}], items: [...] | null, leaf: noteResp | null, loading: bool }
  const [drill, setDrill] = useState({});

  const doSearch = () => {
    if (!query.trim()) return;
    setBusy(true);
    setResults(null);
    setExp(null);
    setNoteData({});
    setDrill({});
    api.search(query).then((resp) => {
      if (resp.success && resp.records) {
        const flat = [];
        for (const [src, items] of Object.entries(resp.records)) {
          for (const item of items) {
            flat.push({ ...item, src });
          }
        }
        flat.sort((a, b) => b.rating - a.rating);
        setResults(deduplicateResults(flat));
      } else {
        setResults([]);
      }
    }).catch(() => setResults([])).finally(() => setBusy(false));
  };

  // Auto-search on mount if query is present (e.g. coming from Home)
  useEffect(() => {
    if (query.trim() && !results && !busy) doSearch();
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  const handleSubmit = (e) => {
    e.preventDefault();
    doSearch();
  };

  // Try to load children using parentId + level; if none, load leaf tariff
  const drillLoad = async (idx, parentId, leafCode, path, childLevel) => {
    setDrill(prev => ({ ...prev, [idx]: { path, items: null, leaf: null, loading: true } }));
    try {
      const resp = await api.browse({ parentId, level: childLevel });
      if (resp.success && resp.records?.length > 0) {
        setDrill(prev => ({ ...prev, [idx]: { path, items: resp.records.sort((a, b) => a.code.localeCompare(b.code)), leaf: null, loading: false } }));
      } else {
        // Leaf — load tariff
        const noteResp = await api.note(leafCode, country || "NG");
        setDrill(prev => ({ ...prev, [idx]: { path, items: null, leaf: noteResp.success ? noteResp : null, loading: false } }));
      }
    } catch {
      setDrill(prev => ({ ...prev, [idx]: { path, items: [], leaf: null, loading: false } }));
    }
  };

  const handleDrillNav = (idx, child) => {
    const dd = drill[idx] || { path: [] };
    const lastId = dd.path.length > 0 ? dd.path[dd.path.length - 1].id : (results[idx].hsCodes?.[0]?.id);
    if (lastId === child.id) return;
    const newPath = [...dd.path, { code: child.code, desc: child.description, level: child.level, id: child.id }];
    drillLoad(idx, child.id, child.code, newPath, child.level + 1);
  };

  const handleExpand = (idx) => {
    if (exp === idx) { setExp(null); return; }
    setExp(idx);
    const r = results[idx];
    const code = r.hsCodes?.[0]?.code || r.code;
    const level = r.hsCodes?.[0]?.level || 0;
    if (code && country && !noteData[code + country]) {
      api.note(code, country).then((resp) => {
        if (resp.success) {
          setNoteData(prev => ({ ...prev, [code + country]: resp }));
        }
      }).catch(() => {});
    }
    // Auto-load children for non-leaf codes using ID-based navigation
    const id = r.hsCodes?.[0]?.id;
    if (level < 4 && !drill[idx] && id) {
      drillLoad(idx, id, code, [], level + 1);
    }
  };

  useEffect(() => {
    if (results && exp != null && country) {
      const r = results[exp];
      const code = r.hsCodes?.[0]?.code || r.code;
      if (code && !noteData[code + country]) {
        api.note(code, country).then((resp) => {
          if (resp.success) setNoteData(prev => ({ ...prev, [code + country]: resp }));
        }).catch(() => {});
      }
    }
  }, [country]);

  const selCountry = countries.find(c => c.code === country);

  return (
    <div className="max-w-[960px] mx-auto pt-8 px-6 pb-16">
      <h2 className="text-2xl font-extrabold tracking-[-0.02em] mb-1.5 text-fg">Classify Commodity</h2>
      <p className="text-fg-sec text-sm mb-6">Describe your product to get predicted HS codes with confidence scores.</p>
      <div className="mb-6">
        <SearchBar query={query} onQueryChange={onQueryChange} busy={busy} onSubmit={handleSubmit} autoFocus />
      </div>

      {busy && (
        <div className="text-center py-[52px]">
          <div className="w-9 h-9 mx-auto mb-3.5 border-[3px] border-border border-t-accent rounded-full animate-spin-loading" />
          <p className="text-fg-dim text-sm">Running classification…</p>
        </div>
      )}

      {results && !busy && (
        <div className="animate-fade-up">
          <div className="flex items-center gap-2.5 mb-3.5 px-[15px] py-[11px] rounded-[9px] bg-surface border border-border">
            <span className="w-1.5 h-1.5 rounded-full bg-success shadow-[0_0_6px_var(--theme-success)]" />
            <span className="text-sm text-fg-sec">
              Found <strong className="text-fg">{results.length} classification{results.length !== 1 ? "s" : ""}</strong> for <strong className="text-accent">"{query}"</strong>
            </span>
          </div>
          <div className="flex flex-col gap-1.5">
            {results.map((r, i) => {
              const code = r.hsCodes?.[0]?.code || r.code;
              const desc = r.hsCodes?.[0]?.description || r.prediction;
              const hierarchy = [...(r.parentHSCodes || []), ...(r.hsCodes || [])].sort((a, b) => a.level - b.level);
              const nd = country ? noteData[code + country] : null;
              const tariffEntries = nd?.tariff?.[0] ? formatTariffEntries(nd.tariff[0]) : [];

              return (
                <div key={i}>
                  <button onClick={() => handleExpand(i)}
                    className={`w-full text-left px-[18px] py-4 cursor-pointer font-sans transition-all duration-150
                      ${exp === i
                        ? 'bg-surface2 border border-accent-border rounded-t-[11px] rounded-b-none'
                        : 'bg-surface border border-border rounded-[11px]'}`}>
                    <div className="flex justify-between items-start gap-3 flex-wrap">
                      <div className="flex-1 min-w-[170px]">
                        <div className="flex items-center gap-2 mb-[5px]">
                          <span className="font-mono text-[15px] font-bold text-accent">{code}</span>
                          {i === 0 && <Badge small>Best match</Badge>}
                        </div>
                        <p className="text-sm text-fg-sec leading-snug m-0">{desc}</p>
                      </div>
                      <div className="flex items-center gap-2.5">
                        <span className={`text-xs font-bold font-mono ${Math.round(r.rating * 100) >= 85 ? 'text-success' : Math.round(r.rating * 100) >= 60 ? 'text-accent' : 'text-warning'}`}>{Math.round(r.rating * 100)}%</span>
                        <ChevronDown size={14} className={`text-fg-dim transition-transform duration-200 ${exp === i ? 'rotate-180' : ''}`} />
                      </div>
                    </div>
                  </button>
                  {exp === i && (() => {
                    const dd = drill[i];
                    const isLeafResult = (r.hsCodes?.[0]?.level || 0) >= 4;
                    const drillLeaf = dd?.leaf;
                    const drillTariffEntries = drillLeaf?.tariff?.[0] ? formatTariffEntries(drillLeaf.tariff[0]) : [];

                    // Unified hierarchy: static parents + drill path
                    const fullHierarchy = [...hierarchy, ...(dd?.path || []).map(p => ({ code: p.code || "", description: p.desc, id: p.id, level: p.level }))];
                    // If drill reached a leaf, add it to hierarchy
                    if (drillLeaf?.records?.[0]) {
                      const lr = drillLeaf.records[0];
                      fullHierarchy.push({ code: lr.code, description: lr.description, id: lr.id, level: lr.level, isLeaf: true });
                    }

                    // Determine which tariff to show
                    const showTariff = isLeafResult ? tariffEntries : drillTariffEntries;
                    const showTariffReady = isLeafResult ? !!nd : !!drillLeaf;

                    return (
                    <div className="p-[18px] bg-surface border border-accent-border border-t-0 rounded-b-[11px] animate-fade-up-fast">
                      <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.06em] mb-2.5">HS Code Hierarchy</div>
                      <div className="flex flex-col mb-3.5">
                        {fullHierarchy.map((h, hi) => {
                          const isLast = hi === fullHierarchy.length - 1 && (isLeafResult || drillLeaf || (!dd?.items));
                          const isClickable = hi < fullHierarchy.length - 1 || (dd?.items && !isLast);
                          // Clicking a hierarchy item navigates back to show its children
                          const handleClick = () => {
                            if (hi < hierarchy.length) {
                              const target = hierarchy[hi];
                              const targetId = target.id || r.hsCodes?.[0]?.id;
                              if (hi === hierarchy.length - 1) {
                                drillLoad(i, targetId, target.code, [], (target.level || 0) + 1);
                              } else {
                                return;
                              }
                            } else {
                              const drillIdx = hi - hierarchy.length;
                              const target = dd.path[drillIdx];
                              const trimmedPath = dd.path.slice(0, drillIdx + 1);
                              drillLoad(i, target.id, target.code, trimmedPath, target.level + 1);
                            }
                          };
                          return (
                            <button key={hi} onClick={isClickable ? handleClick : undefined}
                              className={`flex items-center gap-2.5 py-[7px] px-2.5 rounded-[5px] border-none w-full text-left font-sans
                                ${isLast ? 'bg-accent-dim border-l-[3px] border-l-accent' : 'bg-transparent border-l-[3px] border-l-transparent'}
                                ${isClickable ? 'cursor-pointer' : 'cursor-default'}`}
                              style={{ paddingLeft: `${10 + hi * 22}px` }}>
                              <span className={`font-mono text-[13px] font-semibold min-w-[72px] ${isClickable ? 'text-accent' : (isLast ? 'text-accent' : 'text-fg-dim')}`}>{h.code}</span>
                              <span className={`text-[13px] flex-1 ${isLast ? 'text-fg' : 'text-fg-sec'}`}>{h.description}</span>
                            </button>
                          );
                        })}
                      </div>

                      {/* Loading */}
                      {dd?.loading && (
                        <div className="text-xs text-fg-dim py-2" style={{ paddingLeft: `${fullHierarchy.length * 22}px` }}>Loading…</div>
                      )}

                      {/* Child items below the hierarchy */}
                      {!isLeafResult && dd?.items && !dd.loading && (
                        <div className="flex flex-col gap-[3px]" style={{ paddingLeft: `${Math.min(fullHierarchy.length * 22, 110)}px` }}>
                          {dd.items.length === 0 ? (
                            <div className="text-xs text-fg-dim py-2">No sub-items found.</div>
                          ) : dd.items.map((child, ci) => (
                            <button key={ci} onClick={() => handleDrillNav(i, child)}
                              className="flex items-center gap-2.5 py-[9px] px-3 rounded-[7px] border border-border bg-surface cursor-pointer text-left font-sans transition-all duration-150 hover:border-accent-border hover:bg-surface2">
                              <span className="font-mono text-[13px] font-bold text-accent min-w-[62px]">{child.code}</span>
                              <span className="text-[13px] text-fg flex-1 leading-snug">{child.description}</span>
                              <ChevronRight size={13} className="text-fg-dim shrink-0" />
                            </button>
                          ))}
                        </div>
                      )}

                      {/* Tariffs — shown for leaf results or when drill reaches a leaf */}
                      {(isLeafResult || drillLeaf) && !dd?.loading && (() => {
                        const leafCode = isLeafResult ? code : (drillLeaf?.records?.[0]?.code || code);
                        const leafDesc = isLeafResult ? desc : (drillLeaf?.records?.[0]?.description || desc);
                        return (
                        <div className="mt-3.5">
                          <div className="flex items-center justify-between mb-2.5">
                            <div className="text-[11px] font-bold text-fg-dim uppercase tracking-[0.06em]">
                              Applicable Tariffs {selCountry && <span className="text-accent">— {selCountry.flag} {selCountry.name}</span>}
                            </div>
                            {onCalc && (
                              <button type="button" onClick={() => onCalc(leafCode, leafDesc)}
                                className="flex items-center gap-1.5 px-2.5 py-1 rounded-md bg-accent-dim border border-accent-border text-accent text-[11px] font-semibold cursor-pointer font-sans hover:bg-accent hover:text-btn-text transition-colors duration-150">
                                <Calculator size={12} /> Calculate duties
                              </button>
                            )}
                          </div>
                          {!country ? (
                            <div className="text-sm text-fg-dim py-2">Select a country to view tariffs.</div>
                          ) : country !== "NG" ? (
                            <div className="text-sm text-fg-dim py-2 italic">Tariff data for {selCountry?.name || country} coming soon.</div>
                          ) : showTariff.length > 0 ? (
                            <div className="grid grid-cols-[repeat(auto-fit,minmax(110px,1fr))] gap-2">
                              {showTariff.map(([l, v], ti) => (
                                <div key={ti} className="px-3.5 py-2.5 rounded-lg bg-surface2 border border-border">
                                  <div className="text-[11px] text-fg-dim mb-0.5 uppercase tracking-[0.05em]">{l}</div>
                                  <div className="text-base font-bold font-mono text-fg">{v}</div>
                                </div>
                              ))}
                            </div>
                          ) : showTariffReady ? (
                            <div className="text-xs text-fg-dim py-2">No tariff data for this code.</div>
                          ) : (
                            <div className="text-xs text-fg-dim py-2">Loading tariff data…</div>
                          )}
                        </div>
                        );
                      })()}
                    </div>
                    );
                  })()}
                </div>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}
