import { useState, useEffect, useCallback } from "react";
import { AlertTriangle, X } from "lucide-react";

export default function Toast() {
  const [visible, setVisible] = useState(false);
  const [msg, setMsg] = useState("");

  const show = useCallback((text) => {
    setMsg(text);
    setVisible(true);
    setTimeout(() => setVisible(false), 5000);
  }, []);

  useEffect(() => {
    const handler = () => show("Too many requests — please wait a moment before trying again.");
    window.addEventListener("api:ratelimit", handler);
    return () => window.removeEventListener("api:ratelimit", handler);
  }, [show]);

  if (!visible) return null;

  return (
    <div className="fixed top-4 right-4 z-50 flex items-center gap-3 bg-surface border border-danger/30 rounded-lg shadow-lg px-4 py-3 max-w-sm animate-[slideIn_0.3s_ease-out]">
      <AlertTriangle size={18} className="text-danger shrink-0" />
      <span className="text-sm text-fg">{msg}</span>
      <button onClick={() => setVisible(false)} className="text-fg-dim hover:text-fg ml-auto shrink-0">
        <X size={16} />
      </button>
    </div>
  );
}
