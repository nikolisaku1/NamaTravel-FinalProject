// destinations.js (FINAL VERSION)
// Adds client-side filtering + click-to-prefill reservation destination

const searchInput = document.getElementById("searchDest");
const destinationCards = document.querySelectorAll(".cards .card");

function applyFilter() {
  const query = (searchInput.value || "").toLowerCase();
  destinationCards.forEach(card => {
    const title = card.querySelector(".card-title").textContent.toLowerCase();
    const country = card.querySelector(".card-value").textContent.toLowerCase();
    card.style.display = (title.includes(query) || country.includes(query)) ? "block" : "none";
  });
}

searchInput.addEventListener("input", applyFilter);
applyFilter();

// Clicking a destination sends you to dashboard with prefilled destination
destinationCards.forEach(card => {
  card.addEventListener("click", () => {
    const destinationName = card.querySelector(".card-title").textContent;
    window.location.href = `dashboard.html?deal=${encodeURIComponent(destinationName)}`;
  });
});
