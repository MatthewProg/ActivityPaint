export async function copyToClipboard(text) {
    await navigator.clipboard.writeText(text);
}

export async function copyElementTextToClipboard(selector) {
    const element = document.querySelector(selector);
    await copyToClipboard(element.innerText);
}

export function scrollToTop() {
    window.scrollTo(0, 0);
}