import { useState, useEffect } from "react";

const GA_ID = "UA-159243847-1";
const CONSENT_KEY = "hs_cookie_consent";

function loadGA() {
  if (document.getElementById("ga-script")) return;
  const s = document.createElement("script");
  s.id = "ga-script";
  s.async = true;
  s.src = `https://www.googletagmanager.com/gtag/js?id=${GA_ID}`;
  document.head.appendChild(s);
  window.dataLayer = window.dataLayer || [];
  function gtag() { window.dataLayer.push(arguments); }
  gtag("js", new Date());
  gtag("config", GA_ID);
}

export default function CookieConsent() {
  const [show, setShow] = useState(false);

  useEffect(() => {
    const consent = localStorage.getItem(CONSENT_KEY);
    if (consent === "accepted") {
      loadGA();
    } else if (!consent) {
      setShow(true);
    }
  }, []);

  const accept = () => {
    localStorage.setItem(CONSENT_KEY, "accepted");
    loadGA();
    setShow(false);
  };

  const decline = () => {
    localStorage.setItem(CONSENT_KEY, "declined");
    setShow(false);
  };

  if (!show) return null;

  return (
    <div className="fixed bottom-0 inset-x-0 z-50 p-4">
      <div className="mx-auto max-w-2xl bg-surface border border-border rounded-xl shadow-lg px-5 py-4 flex flex-col sm:flex-row items-start sm:items-center gap-3 animate-[slideIn_0.3s_ease-out]">
        <p className="text-sm text-fg-sec flex-1">
          We use cookies to analyse site usage and improve our service.
        </p>
        <div className="flex gap-2 shrink-0">
          <button
            onClick={decline}
            className="px-3 py-1.5 text-xs rounded-lg border border-border text-fg-dim hover:text-fg hover:border-border-hover transition-colors cursor-pointer"
          >
            Decline
          </button>
          <button
            onClick={accept}
            className="px-3 py-1.5 text-xs rounded-lg bg-accent text-btn-text hover:bg-accent-hover transition-colors cursor-pointer"
          >
            Accept
          </button>
        </div>
      </div>
    </div>
  );
}
