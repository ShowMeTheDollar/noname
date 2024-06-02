//Container8-RoomImage
        const imgArea = document.querySelector('#gridDemo');
        const btn = document.querySelector('.container8-roomImage .next');
        let uploadedImages = [];
        const filesInput = document.querySelector("#uploadfile");
        const url = '/api/UploadImage/upload'; // 根據你的API路徑進行調整
        const spinner = document.querySelector('.spinner-border');

        new Sortable(imgArea, {
            animation: 150,
            ghostClass: 'blue-background-class'
        });

        filesInput.addEventListener("change", async (e) => {
            if (window.File && window.FileReader && window.FileList && window.Blob) {
                spinner.classList.remove('d-none');
                const files = e.target.files;
                for (let i = 0; i < files.length; i++) {
                    if (!files[i].type.match("image")) continue;
                    const picReader = new FileReader();
                    picReader.onload = async (event) => {
                        const picFile = event.target;
                        let formData = new FormData();
                        formData.append('files', files[i]); // 根據後端接收方式調整

                        try {
                            const response = await fetch(url, {
                                method: 'POST',
                                body: formData
                            });
                            const imgUrl = await response.json(); // 假設後端返回圖片URL的列表
                            uploadedImages.push(imgUrl[0]); // 假設返回的是一個URL列表

                            const div = document.createElement("div");
                            div.setAttribute("class", "col-12 col-md-6 col-lg-4 img-box grid-square");
                            div.innerHTML = `<img src="${picFile.result}" alt="">`;
                            div.innerHTML += `<div class="deleteBtn">X</div>`;
                            imgArea.appendChild(div);

                            spinner.classList.add('d-none');
                            fileCount();

                            div.querySelector('.deleteBtn').addEventListener('click', function () {
                                this.parentElement.remove();
                                fileCount();
                            });
                        } catch (error) {
                            console.error(error);
                        }
                    };
                    picReader.readAsDataURL(files[i]);
                }
            } else {
                alert("Your browser does not support File API");
            }
        });

        function fileCount() {
            const imgBoxes = document.querySelectorAll('.img-box').length;
            if (imgBoxes < 6) {
                btn.setAttribute('disabled', 'disabled');
                btn.classList.add('fail');
                btn.classList.remove('accept');
            } else {
                btn.removeAttribute('disabled');
                btn.classList.remove('fail');
                btn.classList.add('accept');
            }
            // 將已上傳圖片URLs存儲到cookie中
            document.cookie = "uploadedImages=" + uploadedImages.join(';') + ";path=/";
        }