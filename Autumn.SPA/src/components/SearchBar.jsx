import { useRef, useEffect, useState, useCallback } from "react";
import { Search } from "lucide-react";
import { api } from "../api";

export default function SearchBar({ query, onQueryChange, busy, onSubmit, autoFocus, large }) {
  const ref = useRef(null);
  const debounceRef = useRef(null);
  const blurRef = useRef(null);
  const [suggestions, setSuggestions] = useState([]);
  const [show, setShow] = useState(false);
  const [activeIdx, setActiveIdx] = useState(-1);

  useEffect(() => {
    if (autoFocus && ref.current) ref.current.focus();
  }, [autoFocus]);

  const fetchSuggestions = useCallback((q) => {
    clearTimeout(debounceRef.current);
    if (!q || q.trim().length < 2) {
      setSuggestions([]);
      setShow(false);
      return;
    }
    debounceRef.current = setTimeout(() => {
      api.products(q.trim()).then((resp) => {
        if (resp.success && resp.results?.length > 0) {
          setSuggestions(resp.results.slice(0, 8));
          setShow(true);
        } else {
          setSuggestions([]);
          setShow(false);
        }
      }).catch(() => {
        setSuggestions([]);
        setShow(false);
      });
    }, 300);
  }, []);

  const handleChange = (e) => {
    const val = e.target.value;
    onQueryChange(val);
    setActiveIdx(-1);
    fetchSuggestions(val);
  };

  const selectSuggestion = (text) => {
    onQueryChange(text);
    setSuggestions([]);
    setShow(false);
    setActiveIdx(-1);
    // Auto-submit after selecting
    setTimeout(() => {
      ref.current?.closest("form")?.requestSubmit();
    }, 0);
  };

  const handleKeyDown = (e) => {
    if (!show || suggestions.length === 0) return;
    if (e.key === "ArrowDown") {
      e.preventDefault();
      setActiveIdx((prev) => (prev < suggestions.length - 1 ? prev + 1 : 0));
    } else if (e.key === "ArrowUp") {
      e.preventDefault();
      setActiveIdx((prev) => (prev > 0 ? prev - 1 : suggestions.length - 1));
    } else if (e.key === "Enter" && activeIdx >= 0) {
      e.preventDefault();
      selectSuggestion(suggestions[activeIdx].text);
    } else if (e.key === "Escape") {
      setShow(false);
      setActiveIdx(-1);
    }
  };

  const handleBlur = () => {
    // Delay to allow click on suggestion
    blurRef.current = setTimeout(() => setShow(false), 150);
  };

  const handleFocus = () => {
    clearTimeout(blurRef.current);
    if (suggestions.length > 0 && query.trim().length >= 2) setShow(true);
  };

  // Highlight the matching portion of a suggestion
  const highlight = (text) => {
    const q = query.trim().toLowerCase();
    const idx = text.toLowerCase().indexOf(q);
    if (idx < 0) return text;
    return (
      <>
        {text.slice(0, idx)}
        <strong className="text-accent font-semibold">{text.slice(idx, idx + q.length)}</strong>
        {text.slice(idx + q.length)}
      </>
    );
  };

  return (
    <form onSubmit={onSubmit} className="w-full">
      <div className="relative">
        <div className={`flex items-center bg-input-bg border-[1.5px] rounded-xl overflow-hidden transition-colors duration-200
          ${query ? 'border-accent-border' : 'border-border'}`}>
          <div className="px-3.5 text-fg-dim flex items-center">
            <Search size={17} />
          </div>
          <input
            ref={ref}
            value={query}
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            onBlur={handleBlur}
            onFocus={handleFocus}
            placeholder="Describe your product — e.g. 'laptop computer'"
            autoComplete="off"
            className={`flex-1 border-none bg-transparent text-fg outline-none font-sans
              ${large ? 'py-[15px] text-[15px]' : 'py-[13px] text-sm'}`}
          />
          <button type="submit" disabled={busy}
            className={`py-[9px] px-[22px] m-[5px] rounded-lg border-none font-bold text-[13px] font-sans transition-all duration-200
              ${busy ? 'bg-surface2 text-fg-dim cursor-wait' : 'bg-grad text-btn-text cursor-pointer'}`}>
            {busy ? "Classifying\u2026" : "Classify"}
          </button>
        </div>

        {show && suggestions.length > 0 && (
          <div className="absolute left-0 right-0 top-full mt-1 rounded-lg border border-border bg-surface shadow-lg z-50 overflow-hidden animate-fade-up-fast">
            {suggestions.map((s, i) => (
              <button
                key={i}
                type="button"
                onMouseDown={(e) => e.preventDefault()}
                onClick={() => selectSuggestion(s.text)}
                onMouseEnter={() => setActiveIdx(i)}
                className={`w-full text-left px-4 py-2.5 text-sm font-sans flex items-center gap-3 border-none cursor-pointer transition-colors duration-100
                  ${i === activeIdx ? 'bg-surface2 text-fg' : 'bg-transparent text-fg-sec'}`}
              >
                <Search size={13} className="text-fg-dim shrink-0" />
                <span className="truncate">{highlight(s.text)}</span>
                {s.value && <span className="ml-auto text-xs text-fg-dim font-mono shrink-0">{s.value}</span>}
              </button>
            ))}
          </div>
        )}
      </div>
    </form>
  );
}
