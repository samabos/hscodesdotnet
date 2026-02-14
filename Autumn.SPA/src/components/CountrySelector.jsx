import { useState, useRef, useEffect } from "react";
import { Globe, ChevronDown } from "lucide-react";

export default function CountrySelector({ value, onChange, countries = [], compact }) {
  const [open, setOpen] = useState(false);
  const selected = countries.find(c => c.code === value);
  const wrapRef = useRef(null);

  useEffect(() => {
    const h = (e) => { if (wrapRef.current && !wrapRef.current.contains(e.target)) setOpen(false); };
    document.addEventListener("mousedown", h);
    return () => document.removeEventListener("mousedown", h);
  }, []);

  return (
    <div ref={wrapRef} className="relative inline-block">
      <button onClick={() => setOpen(!open)}
        className={`flex items-center rounded-lg border-[1.5px] cursor-pointer font-sans transition-all duration-150
          ${compact ? 'gap-1.5 px-2.5 py-1.5' : 'gap-2 px-3.5 py-2'}
          ${value ? 'border-accent-border bg-accent-dim' : 'border-border bg-surface'}`}>
        {selected ? (
          <>
            <span className={compact ? 'text-sm' : 'text-base'}>{selected.flag}</span>
            <span className={`font-semibold text-fg ${compact ? 'text-xs' : 'text-[13px]'}`}>{selected.name}</span>
          </>
        ) : (
          <>
            <Globe size={compact ? 13 : 15} className="text-fg-dim" />
            <span className={`text-fg-dim ${compact ? 'text-xs' : 'text-[13px]'}`}>Select country</span>
          </>
        )}
        <ChevronDown size={12} className={`text-fg-dim transition-transform duration-200 ${open ? 'rotate-180' : ''}`} />
      </button>
      {open && (
        <div className="absolute top-full left-0 mt-1 min-w-[200px] bg-surface border border-border rounded-[10px] shadow-[0_8px_32px_rgba(0,0,0,0.18)] z-[100] overflow-hidden animate-fade-up-fast">
          {countries.map(c => (
            <button key={c.code} onClick={() => { onChange(c.code); setOpen(false); }}
              className={`flex items-center gap-2.5 w-full px-3.5 py-2.5 border-none cursor-pointer font-sans transition-colors duration-100 text-left
                ${c.code === value ? 'bg-accent-dim' : 'bg-transparent hover:bg-surface2'}`}>
              <span className="text-base">{c.flag}</span>
              <div>
                <div className="text-[13px] font-semibold text-fg">{c.name}</div>
                <div className="text-[11px] text-fg-dim">{c.currency} · {c.symbol}</div>
              </div>
              {c.code === value && <div className="ml-auto w-1.5 h-1.5 rounded-full bg-accent" />}
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
