export default function OwlLogo({ size = 30 }) {
  return (
    <div
      className="flex items-center justify-center bg-grad shadow-[0_0_14px_var(--theme-logo-glow)]"
      style={{ width: size, height: size, borderRadius: size * 0.25 }}
    >
      <svg width={size * 0.7} height={size * 0.7} viewBox="0 0 24 24" fill="none">
        <circle cx="7.5" cy="13" r="5" stroke="var(--theme-btn-text)" strokeWidth="2" />
        <circle cx="16.5" cy="13" r="5" stroke="var(--theme-btn-text)" strokeWidth="2" />
        <path d="M12.5 13 C12.5 11, 11.5 11, 11.5 13" stroke="var(--theme-btn-text)" strokeWidth="1.8" strokeLinecap="round" />
        <circle cx="7.5" cy="13" r="1.5" fill="var(--theme-btn-text)" />
        <circle cx="16.5" cy="13" r="1.5" fill="var(--theme-btn-text)" />
        <path d="M4 7.5 L5.5 9.5" stroke="var(--theme-btn-text)" strokeWidth="1.6" strokeLinecap="round" />
        <path d="M20 7.5 L18.5 9.5" stroke="var(--theme-btn-text)" strokeWidth="1.6" strokeLinecap="round" />
      </svg>
    </div>
  );
}
