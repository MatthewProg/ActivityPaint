function resizeCanvas() {
    console.log('resize called');
    const canvas = document.getElementById('editor-canvas');
    const cell = canvas.getElementsByTagName('td')[0];
    const cellWidth = cell.clientWidth + 2;

    canvas.style.setProperty('--canvas-item-width', `${cellWidth}px`);
}

export function init() {
    addEventListener('resize', resizeCanvas);
    resizeCanvas(undefined);
}

export function destroy() {
    removeEventListener('resize', resizeCanvas);
}
