const variants = {
  default: "bg-accent-dim text-accent",
  ai: "bg-[rgba(139,92,246,0.12)] text-[#A78BFA]",
  match: "bg-success-dim text-success",
  synonym: "bg-info-dim text-info",
};

export default function Badge({ children, variant = "default", small }) {
  return (
    <span className={`inline-flex rounded-[5px] font-bold tracking-[0.04em] uppercase whitespace-nowrap
      ${small ? 'px-[7px] py-[2px] text-[10px]' : 'px-2.5 py-[3px] text-[11px]'}
      ${variants[variant] || variants.default}`}>
      {children}
    </span>
  );
}
