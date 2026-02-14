import {
  Fish, Wheat, Droplets, Wine, Pickaxe, Beaker, Recycle, Briefcase,
  TreePine, FileText, Shirt, Footprints, Mountain, Gem, Hammer, Cpu,
  Ship, Glasses, Target, Armchair, Palette
} from "lucide-react";

export const ICON_MAP = {
  "01-05": Fish, "06-14": Wheat, "15": Droplets, "16-24": Wine,
  "25-27": Pickaxe, "28-38": Beaker, "39-40": Recycle, "41-43": Briefcase,
  "44-46": TreePine, "47-49": FileText, "50-63": Shirt, "64-67": Footprints,
  "68-70": Mountain, "71": Gem, "72-83": Hammer, "84-85": Cpu,
  "86-89": Ship, "90-92": Glasses, "93": Target, "94-96": Armchair, "97": Palette,
};

// parentCode maps each section to the Roman numeral used in the DB
export const SECTIONS = [
  { code: "01-05", parentCode: "I", title: "Live Animals & Animal Products" },
  { code: "06-14", parentCode: "II", title: "Vegetable Products" },
  { code: "15", parentCode: "III", title: "Animal or Vegetable Fats & Oils" },
  { code: "16-24", parentCode: "IV", title: "Foodstuffs, Beverages & Tobacco" },
  { code: "25-27", parentCode: "V", title: "Mineral Products" },
  { code: "28-38", parentCode: "VI", title: "Chemical Products" },
  { code: "39-40", parentCode: "VII", title: "Plastics & Rubber" },
  { code: "41-43", parentCode: "VIII", title: "Hides, Skins & Leather" },
  { code: "44-46", parentCode: "IX", title: "Wood & Wood Products" },
  { code: "47-49", parentCode: "X", title: "Paper & Paperboard" },
  { code: "50-63", parentCode: "XI", title: "Textiles & Textile Articles" },
  { code: "64-67", parentCode: "XII", title: "Footwear, Headgear & Umbrellas" },
  { code: "68-70", parentCode: "XIII", title: "Stone, Ceramic & Glass" },
  { code: "71", parentCode: "XIV", title: "Precious Metals & Stones" },
  { code: "72-83", parentCode: "XV", title: "Base Metals & Articles" },
  { code: "84-85", parentCode: "XVI", title: "Machinery & Electrical Equipment" },
  { code: "86-89", parentCode: "XVII", title: "Vehicles, Aircraft & Vessels" },
  { code: "90-92", parentCode: "XVIII", title: "Optical, Medical & Musical Instruments" },
  { code: "93", parentCode: "XIX", title: "Arms & Ammunition" },
  { code: "94-96", parentCode: "XX", title: "Furniture, Toys & Misc. Goods" },
  { code: "97", parentCode: "XXI", title: "Works of Art & Antiques" },
];
