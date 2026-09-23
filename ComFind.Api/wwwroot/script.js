const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");
const resultsContainer = document.getElementById("results");
const status = document.getElementById("status");

searchButton.addEventListener("click", search);

searchInput.addEventListener("keydown", function (event) {
    if (event.key === "Enter") {
        search();
    }
});

async function search() {
    const query = searchInput.value.trim();

    if (!query) {
        status.textContent = "Please enter a search query.";
        resultsContainer.innerHTML = "";
        return;
    }

    status.textContent = "Searching...";
    resultsContainer.innerHTML = "";

    try {
        const response = await fetch(
            `/api/search?q=${encodeURIComponent(query)}`
        );

        if (!response.ok) {
            throw new Error("Search request failed.");
        }

        const data = await response.json();

        status.textContent = `${data.results.length} result(s) found.`;

        displayResults(data.results);

    } catch (error) {
        status.textContent = "Unable to perform search.";
        console.error(error);
    }
}

function displayResults(results) {
    if (results.length === 0) {
        resultsContainer.innerHTML = "<p>No results found.</p>";
        return;
    }

    results.forEach(result => {
        const resultElement = document.createElement("div");

        resultElement.className = "result";

        resultElement.innerHTML = `
            <h2>${escapeHtml(result.title)}</h2>

            <a href="${escapeHtml(result.url)}" target="_blank">
                ${escapeHtml(result.url)}
            </a>

            <p>${escapeHtml(result.snippet)}</p>
        `;

        resultsContainer.appendChild(resultElement);
    });
}

function escapeHtml(value) {
    const div = document.createElement("div");
    div.textContent = value;
    return div.innerHTML;
}