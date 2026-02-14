const BASE = '/api';

let _token = null;
export const setAuthToken = (token) => { _token = token; };

const headers = () => _token ? { Authorization: `Bearer ${_token}` } : {};

const json = async (r) => {
  if (r.status === 429) {
    window.dispatchEvent(new CustomEvent('api:ratelimit'));
    throw new Error('Too many requests. Please wait a moment and try again.');
  }
  return r.json();
};

export const api = {
  search: (keyword) =>
    fetch(`${BASE}/search?keyword=${encodeURIComponent(keyword)}`, { headers: headers() }).then(json),

  browse: ({ code, parentCode, parentId, level } = {}) => {
    const params = new URLSearchParams();
    if (code) params.set('code', code);
    if (parentCode) params.set('parentCode', parentCode);
    if (parentId) params.set('parentId', parentId);
    if (level != null) params.set('level', String(level));
    return fetch(`${BASE}/browse?${params}`, { headers: headers() }).then(json);
  },

  duty: ({ HSCode, Country, ProductDesc, Cost, Freight, Insurance, Currency }) =>
    fetch(`${BASE}/duty?${new URLSearchParams({
      HSCode, Country, ProductDesc: ProductDesc || '',
      Cost: String(Cost), Freight: String(Freight),
      Insurance: String(Insurance), Currency: Currency || 'USD'
    })}`, { headers: headers() }).then(json),

  note: (hscode, country) =>
    fetch(`${BASE}/note/${encodeURIComponent(hscode)}?country=${encodeURIComponent(country)}`, { headers: headers() }).then(json),

  countries: () =>
    fetch(`${BASE}/codelist/countries`, { headers: headers() }).then(json),

  currencies: () =>
    fetch(`${BASE}/codelist/currency`, { headers: headers() }).then(json),

  products: (query) =>
    fetch(`${BASE}/codelist/products/${encodeURIComponent(query || '')}`, { headers: headers() }).then(json),
};
