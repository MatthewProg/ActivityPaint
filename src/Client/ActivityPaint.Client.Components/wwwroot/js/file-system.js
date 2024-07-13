﻿export async function downloadFile(fileName, data) {
    const arrayBuffer = await data.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';

    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}
