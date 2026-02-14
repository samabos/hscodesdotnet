import { Sun, Moon, LogIn, LogOut } from "lucide-react";
import OwlLogo from "./OwlLogo";
import CountrySelector from "./CountrySelector";

export default function Header({ country, setCountry, countries, view, setView, mode, setMode, onReset, auth }) {
  const isAuthenticated = auth?.isAuthenticated ?? false;
  const isLoading = auth?.isLoading ?? false;
  const user = auth?.user ?? null;
  const error = auth?.error ?? null;

  if (error) console.error("Auth0 error:", error);

  return (
    <header className="border-b border-border backdrop-blur-[16px] bg-header-bg sticky top-0 z-50">
      <div className="max-w-[1200px] mx-auto px-6 flex items-center justify-between h-14">
        <div className="flex items-center gap-2.5 cursor-pointer" onClick={onReset}>
          <OwlLogo size={30} />
          <span className="text-base font-bold text-fg tracking-[-0.02em]">HS<span className="text-accent">.</span>Codes</span>
        </div>
        <div className="flex items-center gap-1">
          <CountrySelector value={country} onChange={setCountry} countries={countries} compact />
          <div className="w-px h-[18px] bg-border mx-1" />
          {[["home", "Home"], ["results", "Classify"], ["calculator", "Calculator"], ["browse", "Browse"]].map(([k, l]) => (
            <button key={k} onClick={() => setView(k)}
              className={`px-[11px] py-1.5 rounded-[7px] border-none font-sans text-xs font-medium cursor-pointer transition-all duration-150
                ${view === k ? 'bg-accent-dim text-accent' : 'bg-transparent text-fg-dim'}`}>
              {l}
            </button>
          ))}
          <div className="w-px h-[18px] bg-border mx-1" />
          <button onClick={() => setMode(mode === "dark" ? "light" : "dark")} title={mode === "dark" ? "Light mode" : "Dark mode"}
            className="w-[30px] h-[30px] rounded-[7px] border border-border bg-surface text-fg-sec flex items-center justify-center cursor-pointer transition-all duration-200">
            {mode === "dark" ? <Sun size={13} /> : <Moon size={13} />}
          </button>

          {auth && !isLoading && (
            <>
              <div className="w-px h-[18px] bg-border mx-1" />
              {isAuthenticated ? (
                <button
                  onClick={() => auth.logout({ logoutParams: { returnTo: window.location.origin } })}
                  title="Sign out"
                  className="flex items-center gap-1.5 px-2.5 py-1.5 rounded-[7px] border border-border bg-surface text-fg-sec text-xs font-medium cursor-pointer transition-all duration-200 hover:border-border-hover"
                >
                  {user?.picture && (
                    <img src={user.picture} alt="" className="w-5 h-5 rounded-full" referrerPolicy="no-referrer" />
                  )}
                  <LogOut size={13} />
                </button>
              ) : (
                <button
                  onClick={() => auth.loginWithRedirect({ authorizationParams: { connection: "google-oauth2" } })}
                  className="flex items-center gap-1.5 px-2.5 py-1.5 rounded-[7px] border border-border bg-surface text-fg-sec text-xs font-medium cursor-pointer transition-all duration-200 hover:border-border-hover"
                >
                  <LogIn size={13} />
                  <span>Sign in</span>
                </button>
              )}
            </>
          )}
        </div>
      </div>
    </header>
  );
}
