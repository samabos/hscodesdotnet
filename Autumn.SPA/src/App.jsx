import { useState, useEffect, useContext } from "react";
import { Auth0Context } from "@auth0/auth0-react";
import { api, setAuthToken } from "./api";
import Header from "./components/Header";
import Home from "./components/Home";
import SearchView from "./components/SearchView";
import CalcView from "./components/CalcView";
import BrowseView from "./components/BrowseView";
import Toast from "./components/Toast";
import CookieConsent from "./components/CookieConsent";

export default function App() {
  const [mode, setMode] = useState("light");
  const [view, setView] = useState("home");
  const [query, setQuery] = useState("");
  const [country, setCountry] = useState("NG");
  const [countries, setCountries] = useState([]);
  const [calcInit, setCalcInit] = useState({ hscode: "", product: "" });

  // Only use Auth0 context when env vars are configured
  const authEnabled = !!(import.meta.env.VITE_AUTH0_DOMAIN && import.meta.env.VITE_AUTH0_CLIENT_ID);
  const rawAuth = useContext(Auth0Context);
  const auth = authEnabled ? rawAuth : null;

  useEffect(() => {
    if (!auth?.isAuthenticated || !auth.getAccessTokenSilently) return;
    auth.getAccessTokenSilently().then(setAuthToken).catch(() => {});
  }, [auth?.isAuthenticated]);

  useEffect(() => {
    document.documentElement.classList.toggle("dark", mode === "dark");
  }, [mode]);

  useEffect(() => {
    api.countries().then((resp) => {
      if (resp.success && resp.records) {
        setCountries(resp.records);
      }
    }).catch(() => {});
  }, []);

  const onReset = () => {
    setView("home");
    setQuery("");
  };

  const goToCalc = (hscode, product) => {
    setCalcInit({ hscode: hscode || "", product: product || "" });
    setView("calculator");
  };

  const goToSearch = (product) => {
    setQuery(product || "");
    setView("results");
  };

  return (
    <div className="min-h-screen bg-bg text-fg font-sans transition-colors duration-300">
      <Header
        country={country}
        setCountry={setCountry}
        countries={countries}
        view={view}
        setView={setView}
        mode={mode}
        setMode={setMode}
        onReset={onReset}
        auth={auth}
      />

      {view === "home" && (
        <Home
          query={query}
          onQueryChange={setQuery}
          busy={false}
          onSearch={() => {}}
          onNavigate={setView}
        />
      )}

      {view === "results" && (
        <SearchView
          query={query}
          onQueryChange={setQuery}
          country={country}
          countries={countries}
          setCountry={setCountry}
          onCalc={goToCalc}
        />
      )}

      {view === "calculator" && (
        <CalcView
          key={calcInit.hscode + calcInit.product}
          country={country}
          countries={countries}
          setCountry={setCountry}
          onSearchCode={goToSearch}
          initialHscode={calcInit.hscode}
          initialProduct={calcInit.product}
        />
      )}

      {view === "browse" && (
        <BrowseView
          country={country}
          countries={countries}
          setCountry={setCountry}
        />
      )}

      <Toast />
      <CookieConsent />

      <footer className="border-t border-border px-6 py-4 text-center text-xs text-fg-dim">
        © 2025 HS.Codes — Open source commodity classification.
      </footer>
    </div>
  );
}
