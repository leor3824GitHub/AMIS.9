// File download helper for CSV/Excel exports
window.downloadFile = function (filename, base64Content, contentType)
{
    const linkSource = `data:${contentType};base64,${base64Content}`;
    const downloadLink = document.createElement('a');
    downloadLink.href = linkSource;
    downloadLink.download = filename;
    downloadLink.click();
};
