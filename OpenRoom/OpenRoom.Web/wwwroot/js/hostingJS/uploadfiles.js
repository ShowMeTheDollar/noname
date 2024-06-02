document.addEventListener('DOMContentLoaded', function () {
    const input = document.querySelector('input[type="file"]');
    const preview = document.querySelector('.preview');

    input.addEventListener('change', updateImageDisplay);

    function updateImageDisplay() {
        const curFiles = input.files;
        preview.innerHTML = ''; // 清空預覽區域

        const gallery = document.createElement('div');
        gallery.classList.add('gallery');
        preview.appendChild(gallery);

        for (const file of curFiles) {
            if (validFileType(file)) {
                const imageContainer = document.createElement('div');
                imageContainer.classList.add('img-container');

                const image = document.createElement('img');
                image.src = URL.createObjectURL(file);

                const deleteBtn = document.createElement('button');
                deleteBtn.innerText = '刪除';
                deleteBtn.onclick = function () {
                    imageContainer.remove();
                };

                imageContainer.appendChild(image);
                imageContainer.appendChild(deleteBtn);
                gallery.appendChild(imageContainer);
            }
        }

        // 這裡可以添加拖拽排序的JavaScript代碼
    }

    const fileTypes = [
        'image/jpeg',
        'image/pjpeg',
        'image/png'
    ];

    function validFileType(file) {
        return fileTypes.includes(file.type);
    }
});