// Simple fetch helper used by all pages
// If you run the API on a different port, change BASE_URL.
const BASE_URL = "https://localhost:49152";

//5001
async function apiFetch(path, options = {}) {
  const url = `${BASE_URL}${path}`;

  const res = await fetch(url, {
    method: options.method || "GET",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {})
    },
    body: options.body !== undefined ? JSON.stringify(options.body) : undefined
  });

  const isJson = (res.headers.get("content-type") || "").includes("application/json");
  const data = isJson ? await res.json() : await res.text();

  if (!res.ok) {
    const message =
      typeof data === "string"
        ? data
        : (data.title || data.message || JSON.stringify(data));
    throw new Error(message);
  }
  return data;
}

const BookingApi = {
  list: () => apiFetch("/api/bookings"),
  get: (id) => apiFetch(`/api/bookings/${id}`),
  create: (booking) => apiFetch("/api/bookings", { method: "POST", body: booking }),
  update: (id, booking) => apiFetch(`/api/bookings/${id}`, { method: "PUT", body: booking }),
  remove: (id) => apiFetch(`/api/bookings/${id}`, { method: "DELETE" })
};

const SkyApi = {
  // Backend proxies Skyscanner so your API key stays secret.
  // Example: /api/skyscanner/indicative?origin=TIA&destination=ROM&date=2026-03-15
  indicative: (origin, destination, date) =>
    apiFetch(`/api/skyscanner/indicative?origin=${encodeURIComponent(origin)}&destination=${encodeURIComponent(destination)}&date=${encodeURIComponent(date)}`)
};
