function searchTOC(text) {
    text = text.toLowerCase();
    document.querySelectorAll("#toc li").forEach(li => {
        li.style.display = li.innerText.toLowerCase().includes(text) ? "" : "none";
    });
}



// Sidebar TOC search/filter
const searchInput = document.getElementById('searchInput');
const tocLinks = document.querySelectorAll('#toc a');

searchInput.addEventListener('input', function() {
  const query = this.value.toLowerCase().trim();

  tocLinks.forEach(link => {
    const text = link.textContent.toLowerCase();
    if (text.includes(query)) {
      link.style.display = ''; // show link
    } else {
      link.style.display = 'none'; // hide link
    }
  });
});

function exportPDF() {
  // Scroll to top to avoid rendering glitches
  window.scrollTo(0, 0);

  // Open browser print dialog
  window.print();
}

 // Set today's date in "DD-MMM-YYYY" format
  const today = new Date();
  const options = { day: '2-digit', month: 'short', year: 'numeric' };
  const formattedDate = today.toLocaleDateString('en-GB', options).replace(/ /g, '-');
  document.body.setAttribute('data-date', formattedDate);