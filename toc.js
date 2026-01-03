// Generate TOC
const toc = document.getElementById("toc");
document.querySelectorAll(".content-inner h2").forEach(h => {
  const link = document.createElement("a");
  link.className = "nav-link";
  link.href = `#${h.id}`;
  link.textContent = h.textContent;
  toc.appendChild(link);
});



// Search filter
document.getElementById("searchInput").addEventListener("input", e => {
  const q = e.target.value.toLowerCase();
  document.querySelectorAll(".sidebar-nav a").forEach(a => {
    a.style.display = a.textContent.toLowerCase().includes(q) ? "" : "none";
  });
});
