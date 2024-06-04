const canvasId = 'editor-canvas';
const currentSettings = {
    isDarkMode: false,
    brushSize: 1,
    selectedTool: 0,
    selectedIntensity: 0
};

function getColors(isDarkMode = false) {
    return {
        '--canvas-item-level0-color': isDarkMode ? '#161b22' : '#ebedf0',
        '--canvas-item-level1-color': isDarkMode ? '#0e4429' : '#9be9a8',
        '--canvas-item-level2-color': isDarkMode ? '#006d32' : '#40c463',
        '--canvas-item-level3-color': isDarkMode ? '#26a641' : '#30a14e',
        '--canvas-item-level4-color': isDarkMode ? '#39d353' : '#216e39'
    }
}

export function init() {
    addEventListener('mousemove', handleMouseMove);
    addEventListener('mousedown', handleMouseDown);
}

export function updateSettings(settings) {
    currentSettings.isDarkMode = settings.isDarkMode;
    currentSettings.brushSize = settings.brushSize;
    currentSettings.selectedTool = settings.selectedTool;
    currentSettings.selectedIntensity = settings.selectedIntensity;

    const canvas = document.getElementById(canvasId);
    const colors = getColors(settings.isDarkMode);
    for (const key in colors) {
        canvas.style.setProperty(key, colors[key]);
    }
}

export function destroy() {
    removeEventListener('mousemove', handleMouseMove);
    removeEventListener('mousedown', handleMouseDown);
}

function handleMouseDown(e) {
    if ((e.buttons & 1) !== 1) { // LMB only
        return;
    }

    const canvas = document.getElementById(canvasId);
    const cell = getCellIndex(canvas, e.clientX, e.clientY);

    if (!cell) {
        return;
    }

    paint(canvas, cell.indexX, cell.indexY);
}

function handleMouseMove(e) {
    const canvas = document.getElementById(canvasId);
    const cell = getCellIndex(canvas, e.clientX, e.clientY);

    if (!cell) {
        resetHover(canvas);
        return;
    }

    if ((e.buttons & 1) !== 1) { // LMB only
        hover(canvas, cell.indexX, cell.indexY);
        return;
    }

    paint(canvas, cell.indexX, cell.indexY);
}

function hover(canvas, indexX, indexY) {
    const cell = canvas.querySelector(`#cell-${indexX}-${indexY} > div`);
    if (!cell) {
        resetHover(canvas);
        return;
    }

    resetHover(canvas, [cell]);

    if (!cell.classList.contains('hover')) {
        cell.classList.add('hover');
    }

    cell.dataset['level'] = getCurrentIntensity();
}

function resetHover(canvas, skipCells = []) {
    const skipSet = new Set(skipCells);
    const oldCells = canvas.querySelectorAll('div.hover');
    for (let i = 0; i < oldCells.length; i++) {
        const oldCell = oldCells[i];

        if (skipSet.has(oldCell)) {
            continue;
        }

        oldCell.classList.remove('hover');
    }
}

function paint(canvas, indexX, indexY) {
    const cell = canvas.querySelector(`#cell-${indexX}-${indexY}`);
    if (!cell) {
        return;
    }

    cell.dataset['level'] = getCurrentIntensity();
}

function getCellIndex(canvas, x, y) {
    const canvasRect = canvas.getBoundingClientRect();
    if (!pointInRect(x, y, canvasRect)) {
        return undefined;
    }

    const relX = x - canvasRect.x;
    const relY = y - canvasRect.y;

    const blockSize = canvasRect.width / 54;

    const indexX = Math.min(Math.floor(relX / blockSize), 53);
    const indexY = Math.min(Math.floor(relY / blockSize), 6);

    return { indexX, indexY };
}

function pointInRect(x, y, rect) {
    return x >= rect.x
        && x <= rect.x + rect.width
        && y >= rect.y
        && y <= rect.y + rect.height;
}

function getCurrentIntensity() {
    if (currentSettings.selectedTool == 0) { // Brush
        return currentSettings.selectedIntensity;
    }

    if (currentSettings.selectedTool == 1) { // Eraser
        return 0;
    }

    return 0;
}
