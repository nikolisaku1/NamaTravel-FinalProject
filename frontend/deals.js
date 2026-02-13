// deals.js (FINAL VERSION)
// When you click a deal, go to dashboard and prefill destination using URL param (no localStorage)

document.querySelectorAll(".card").forEach(card => {
  card.addEventListener("click", () => {
    const destinationName = card.querySelector(".card-title").textContent;
    window.location.href = `dashboard.html?deal=${encodeURIComponent(destinationName)}`;
  });
});
