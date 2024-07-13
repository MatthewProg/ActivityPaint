const canvasId = 'paint-canvas';
const tools = {
    brush: 0,
    eraser: 1,
    fill: 2
};
const currentSettings = {
    isDarkMode: false,
    brushSize: 1,
    selectedTool: 0,
    selectedIntensity: 0
};

let canvas = undefined;

function getColors(isDarkMode = false) {
    return {
        '--canvas-background-color': isDarkMode ? '#0d1117' : '#ffffff',
        '--canvas-item-level0-color': isDarkMode ? '#161b22' : '#ebedf0',
        '--canvas-item-level1-color': isDarkMode ? '#0e4429' : '#9be9a8',
        '--canvas-item-level2-color': isDarkMode ? '#006d32' : '#40c463',
        '--canvas-item-level3-color': isDarkMode ? '#26a641' : '#30a14e',
        '--canvas-item-level4-color': isDarkMode ? '#39d353' : '#216e39'
    }
}

export function init() {
    canvas = document.getElementById(canvasId);
    canvas.addEventListener('mousemove', handleMouseMove);
    canvas.addEventListener('mousedown', handleMouseDown);
    canvas.addEventListener('mouseleave', handleMouseLeave);
}

export function updateSettings(settings) {
    canvas = canvas ?? document.getElementById(canvasId);

    currentSettings.isDarkMode = settings.isDarkMode;
    currentSettings.brushSize = settings.brushSize;
    currentSettings.selectedTool = settings.selectedTool;
    currentSettings.selectedIntensity = settings.selectedIntensity;

    const colors = getColors(settings.isDarkMode);
    for (const key in colors) {
        canvas.style.setProperty(key, colors[key]);
    }
}

export function serializeCanvas() {
    const cells = canvas.querySelectorAll('td[data-doy]').values();
    const cellsData = Array.from(cells, c => {
        const doy = parseInt(c.dataset['doy']);
        const level = parseInt(c.dataset['level'] ?? 0);
        return [doy, level];
    });

    const data = new Map(cellsData);

    const sorted = [...data.entries()].sort((a, b) => {
        if (a[0] > b[0]) return 1;
        if (a[0] < b[0]) return -1;
        return 0;
    });

    const serialized = sorted.map(x => x[1]);

    return serialized;
}

export function resetCanvas() {
    const cells = canvas.querySelectorAll('td[data-level]').values();

    cells.forEach(c => c.dataset['level'] = '0');
}

export function destroy() {
    canvas.removeEventListener('mousemove', handleMouseMove);
    canvas.removeEventListener('mousedown', handleMouseDown);
    canvas.removeEventListener('mouseleave', handleMouseLeave);
}

function handleMouseDown(e) {
    if (!isLMBPressed(e)) {
        return;
    }

    const cells = getCellsInRange(e.clientX, e.clientY);

    if (cells.length === 0) {
        return;
    }

    if (currentSettings.selectedTool === tools.fill) {
        const cell = cells[0];
        fill(cell.indexX, cell.indexY, getCurrentIntensity());
    } else {
        paint(cells);
    }
}

function handleMouseMove(e) {
    const cells = getCellsInRange(e.clientX, e.clientY);

    if (cells.length === 0) {
        resetHover();
        return;
    }

    if (!isLMBPressed(e)) {
        hover(currentSettings.selectedTool === tools.fill ? [cells[0]] : cells);
        return;
    }

    paint(cells);
}

function handleMouseLeave(e) {
    resetHover();
}

function hover(cells) {
    const cellElements = [];
    for (const { indexX, indexY } of cells) {
        const cell = canvas.querySelector(`#cell-${indexX}-${indexY} > div`);
        if (!cell) {
            continue;
        }
        cellElements.push(cell);
    }

    resetHover(cellElements);

    if (cellElements.length === 0) {
        return;
    }

    for (const cell of cellElements) {
        if (!cell.classList.contains('hover')) {
            cell.classList.add('hover');
        }

        cell.dataset['level'] = getCurrentIntensity();
    }
}

function resetHover(skipCells = []) {
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

function fill(indexX, indexY, to, from = undefined, visitedSet = new Set()) {
    const key = `#cell-${indexX}-${indexY}`;
    if (visitedSet.has(key)) {
        return;
    }

    visitedSet.add(key);

    const cell = canvas.querySelector(key);
    if (!cell) {
        return;
    }

    const cellLevel = cell.dataset['level'] ?? 0;
    const fromValid = from ?? cellLevel;

    if (cellLevel != fromValid) {
        return;
    }

    cell.dataset['level'] = to;

    if (indexX > 0) fill(indexX - 1, indexY, to, fromValid, visitedSet);
    if (indexY > 0) fill(indexX, indexY - 1, to, fromValid, visitedSet);
    if (indexX < 52) fill(indexX + 1, indexY, to, fromValid, visitedSet);
    if (indexY < 6) fill(indexX, indexY + 1, to, fromValid, visitedSet);
}

function paint(cells) {
    for (const { indexX, indexY } of cells) {
        const cell = canvas.querySelector(`#cell-${indexX}-${indexY}`);
        if (!cell) {
            continue;
        }

        cell.dataset['level'] = getCurrentIntensity();
    }
}

function getCellsInRange(x, y) {
    const canvasRect = canvas.getBoundingClientRect();
    const blockSize = canvasRect.width / 54.0;
    const brushSize = getCurrentBrushSize();

    const brushSizePx = brushSize * blockSize;
    const brushRadiusPx = (brushSize / 2.0) * blockSize;

    const selectRect = new DOMRect(x - brushRadiusPx, y - brushRadiusPx, brushSizePx, brushSizePx);

    if (!rectsOverlap(canvasRect, selectRect)) {
        return [];
    }

    const relRect = new DOMRect(selectRect.x - canvasRect.x, selectRect.y - canvasRect.y, selectRect.width, selectRect.height);

    const clampedLeft = Math.max(0, relRect.left) / blockSize;
    const clampedRight = Math.min(canvasRect.width, relRect.right) / blockSize;
    const clampedTop = Math.max(0, relRect.top) / blockSize;
    const clampedBottom = Math.min(canvasRect.height, relRect.bottom) / blockSize;

    const minX = Math.max(0, clampedLeft);
    const maxX = Math.min(54, clampedRight);
    const minY = Math.max(0, clampedTop);
    const maxY = Math.min(7, clampedBottom);

    const cellsInFoucs = [];
    for (let centerX = Math.round(minX) + 0.5; centerX < maxX; centerX++) {
        for (let centerY = Math.round(minY) + 0.5; centerY < maxY; centerY++) {
            const indexX = Math.floor(centerX);
            const indexY = Math.floor(centerY);
            cellsInFoucs.push({ indexX, indexY });
        }
    }
    return cellsInFoucs;
}

function rectsOverlap(rect1, rect2) {
    return !(rect1.right < rect2.left ||
             rect1.left > rect2.right ||
             rect1.bottom < rect2.top ||
             rect1.top > rect2.bottom)
}

const getCurrentBrushSize = () => currentSettings.selectedTool === tools.fill ? 1 : currentSettings.brushSize;

const getCurrentIntensity = () => currentSettings.selectedTool === tools.eraser ? 0 : currentSettings.selectedIntensity;

const isLMBPressed = (e) => (e.buttons & 1) === 1;
