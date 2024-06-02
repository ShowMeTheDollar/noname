const input = document.querySelector('input');
const preview = document.querySelector('.preview');

input.addEventListener('change', updateImageDisplay);

function updateImageDisplay() {
    while (preview.firstChild) {
        preview.removeChild(preview.firstChild);
    }

    const curFiles = input.files;

    if (curFiles.length === 0) {
        const para = document.createElement('p');
        para.textContent = 'No files currently selected for upload';
        preview.appendChild(para);
    } else {
        const container = document.createElement('div');
        container.className = 'image-container'; // 可以用來定義樣式，例如圖片的排列
        preview.appendChild(container);

        for (const file of curFiles) {
            if (validFileType(file)) {
                const image = document.createElement('img');
                image.src = URL.createObjectURL(file);
                image.style.marginRight = '20px'; // 添加右邊距20px
                image.style.marginBottom = '20px'; // 添加底部邊距20px，使得圖片在垂直和水平方向上都有間隔
                container.appendChild(image);
            } else {
                const para = document.createElement('p');
                para.textContent = `File not supported: ${file.name}`;
                preview.appendChild(para);
            }
        }
    }
}

const fileTypes = [
    'image/jpeg',
    'image/pjpeg',
    'image/png',
    'image/webp' // 支援webp格式
];

function validFileType(file) {
    return fileTypes.includes(file.type);
}
