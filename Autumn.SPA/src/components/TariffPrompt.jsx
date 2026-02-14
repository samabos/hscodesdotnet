import { Globe } from "lucide-react";
import CountrySelector from "./CountrySelector";

export default function TariffPrompt({ onSelect, countries }) {
  return (
    <div className="flex items-center gap-2.5 p-3 px-4 rounded-[9px] bg-warning-dim border border-warning/20">
      <Globe size={16} className="text-warning" />
      <span className="text-[13px] text-fg flex-1">Select a country to view applicable tariffs and duties</span>
      <CountrySelector value="" onChange={onSelect} countries={countries} compact />
    </div>
  );
}
