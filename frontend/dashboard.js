// dashboard.js (FINAL VERSION - uses Web API + database, not localStorage)

// If user came from deals page (we pass destination in URL now)
const params = new URLSearchParams(window.location.search);
const selectedDeal = params.get("deal");
if (selectedDeal) {
  document.getElementById("destination").value = selectedDeal;
}

// DOM elements
const bookingForm = document.getElementById("bookingForm");
const tableBody = document.getElementById("bookingTableBody");

const totalBookingsEl = document.getElementById("totalBookings");
const totalTravelersEl = document.getElementById("totalTravelers");
const lastDestinationEl = document.getElementById("lastDestination");

// Local UI state
let bookings = [];
let editingId = null;

function setStats() {
  totalBookingsEl.textContent = bookings.length;
  totalTravelersEl.textContent = bookings.reduce((sum, b) => sum + b.travelers, 0);
  lastDestinationEl.textContent = bookings.length ? bookings[bookings.length - 1].destination : "-";
}

function renderTable() {
  tableBody.innerHTML = "";

  bookings.forEach(b => {
    const row = document.createElement("tr");
    row.innerHTML = `
      <td>${escapeHtml(b.clientName)}</td>
      <td>${escapeHtml(b.destination)}</td>
      <td>${escapeHtml(b.travelDate)}</td>
      <td>${b.travelers}</td>
      <td>
        <button class="btn" data-action="edit" data-id="${b.id}" type="button">Edit</button>
        <button class="btn" data-action="delete" data-id="${b.id}" type="button" style="margin-left:8px;background:#ef4444;">Delete</button>
      </td>
    `;
    tableBody.appendChild(row);
  });

  setStats();
}

function escapeHtml(str) {
  return String(str ?? "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    //.replaceAll(""", "&quot;")
    .replaceAll("'", "&#039;");
}

async function loadBookings() {
  try {
    bookings = await BookingApi.list();
    renderTable();
  } catch (e) {
    alert("Could not load bookings. Is the API running?\n\n" + e.message);
  }
}

function setFormMode(isEditing) {
  const button = bookingForm.querySelector("button[type=submit]");
  button.textContent = isEditing ? "Save Changes" : "Add Reservation";
}

function fillForm(b) {
  document.getElementById("clientName").value = b.clientName;
  document.getElementById("destination").value = b.destination;
  document.getElementById("travelDate").value = b.travelDate;
  document.getElementById("travelers").value = b.travelers;
}

function resetForm() {
  bookingForm.reset();
  editingId = null;
  setFormMode(false);
}

bookingForm.addEventListener("submit", async (event) => {
  event.preventDefault();

  const bookingPayload = {
    clientName: document.getElementById("clientName").value.trim(),
    destination: document.getElementById("destination").value.trim(),
    travelDate: document.getElementById("travelDate").value,
    travelers: Number(document.getElementById("travelers").value)
  };

  try {
    if (editingId) {
      await BookingApi.update(editingId, bookingPayload);
    } else {
      await BookingApi.create(bookingPayload);
    }
    await loadBookings();
    resetForm();
  } catch (e) {
    alert(e.message);
  }
});

// Edit/Delete buttons
tableBody.addEventListener("click", async (e) => {
  const btn = e.target.closest("button[data-action]");
  if (!btn) return;

  const id = Number(btn.dataset.id);
  const action = btn.dataset.action;

  if (action === "edit") {
    const booking = bookings.find(x => x.id === id);
    if (!booking) return;
    editingId = id;
    fillForm(booking);
    setFormMode(true);
    window.scrollTo({ top: 0, behavior: "smooth" });
    return;
  }

  if (action === "delete") {
    if (!confirm("Delete this reservation?")) return;
    try {
      await BookingApi.remove(id);
      await loadBookings();
      resetForm();
    } catch (e) {
      alert(e.message);
    }
  }
});

loadBookings();
