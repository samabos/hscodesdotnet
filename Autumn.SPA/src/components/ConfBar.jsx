export default function ConfBar({ v }) {
  const pct = Math.round(v * 100);
  const barColor = pct >= 85 ? 'bg-success' : pct >= 60 ? 'bg-accent' : 'bg-[#e8a820]';
  const textColor = pct >= 85 ? 'text-success' : pct >= 60 ? 'text-accent' : 'text-[#e8a820]';
  return (
    <div className="flex items-center gap-[7px] min-w-[90px]">
      <div className="flex-1 h-[5px] rounded-[3px] bg-surface2 overflow-hidden">
        <div className={`h-full rounded-[3px] transition-[width] duration-600 ease ${barColor}`}
             style={{ width: `${pct}%` }} />
      </div>
      <span className={`text-[11px] font-bold font-mono min-w-7 text-right ${textColor}`}>{pct}%</span>
    </div>
  );
}
