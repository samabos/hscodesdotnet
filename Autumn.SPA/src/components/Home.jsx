import { Search, Calculator, LayoutGrid, Zap, ArrowRight } from "lucide-react";
import SearchBar from "./SearchBar";

function IconBox({ icon: Icon }) {
  return (
    <div className="w-9 h-9 rounded-lg bg-accent-dim border border-accent-border flex items-center justify-center shrink-0">
      <Icon size={18} className="text-accent" strokeWidth={1.8} />
    </div>
  );
}

export default function Home({ query, onQueryChange, busy, onSearch, onNavigate }) {
  const handleSubmit = (e) => {
    e.preventDefault();
    if (query.trim()) onNavigate("results");
  };

  const cards = [
    { icon: Search, title: "Import Classification", desc: "AI-powered search with predictions, synonym matching, and hierarchical HS code navigation.", go: "results" },
    { icon: Calculator, title: "Duty Calculator", desc: "Calculate import duties, VAT, levies, and total landed cost for your imports.", go: "calculator" },
    { icon: LayoutGrid, title: "Browse HS Codes", desc: "Navigate the complete Harmonized System — 21 sections, 96 chapters, 5000+ subheadings.", go: "browse" },
  ];

  return (
    <div className="max-w-[1200px] mx-auto px-6">
      <div className="text-center pt-[84px] pb-12">
        <div className="inline-flex items-center gap-2 px-3.5 py-[5px] rounded-full border border-border bg-surface mb-[22px] text-xs text-fg-sec">
          <Zap size={12} className="text-accent" strokeWidth={2.5} /> AI-Powered Classification
        </div>
        <h1 className="text-[clamp(30px,5vw,52px)] font-extrabold leading-[1.1] tracking-[-0.035em] mb-4 text-fg">
          Commodity Codes &<br />Tariff Classification
        </h1>
        <p className="text-base text-fg-sec max-w-[500px] mx-auto mb-8 leading-relaxed">
          Classify commodities, calculate duties & taxes, and navigate the Harmonized System.
        </p>
        <div className="max-w-[560px] mx-auto">
          <SearchBar query={query} onQueryChange={onQueryChange} busy={busy} onSubmit={handleSubmit} large />
        </div>
      </div>
      <div className="grid grid-cols-[repeat(auto-fit,minmax(250px,1fr))] gap-3 pb-16">
        {cards.map((c, i) => (
          <button key={i} onClick={() => onNavigate(c.go)}
            className="p-[22px] rounded-[13px] border border-border bg-surface text-left cursor-pointer font-sans transition-all duration-200 hover:border-accent hover:-translate-y-0.5">
            <div className="mb-3.5"><IconBox icon={c.icon} /></div>
            <div className="text-[15px] font-bold mb-[5px] text-fg">{c.title}</div>
            <div className="text-[13px] text-fg-sec leading-normal">{c.desc}</div>
            <div className="mt-3 text-accent text-xs font-semibold flex items-center gap-[5px]">
              Get started <ArrowRight size={12} strokeWidth={2.5} />
            </div>
          </button>
        ))}
      </div>
    </div>
  );
}
