// Vue application
const { createApp } = Vue;

createApp({
    data() {
        return {
            currentContainer: 1, // 控制顯示哪個container
            memberName: '',
            map: null,
            geocoder: null,
            marker: null,
            selectedRoomCategory: "",
            roomCategoryMap: {
                apartment: 1,
                house: 2,
                home: 3,
                luxury: 4,
                garden: 5,
                bnb: 6,
                cabin: 7,
                camp: 8,
                camperVan: 9,
            },
            amenityMap: {
                wifi: 1,
                tv: 2,
                kitchen: 3,
                laundry: 4,
                parking: 5,
                ac: 6,
                pool: 7,
                tub: 8,
                outdoor: 9,
                grill: 10,
                deck: 11,
                fire: 12,
                smoke: 13,
                medical: 14,
                alarm: 15,
                extinguisher: 16,
                cctv: 17,
                weapon: 18,
                animal: 19
                // Continue mapping for other amenities...
            },
            // 假設我們有一個 formData 對象來存儲表單的所有資料
            formData: {
                //memberId: ('@@memberId'),
                //memberId: parseInt(Cookies.get('memberId') || '0', 10), // Directly inject MemberId here// Replace '@@memberId' with actual memberId from your logic

                //roomCategory='',// 正確初始化 formData 屬性(for localstorage)
                //roomCategory: getCookie('roomCategory') || '', // Load from cookie
                roomCategory: Cookies.get('roomCategory') || '', // Load from cookie
                address: {
                    //country: getCookie('country') || '',
                    country: Cookies.get('country') || '',
                    //streetAddress: getCookie('streetAddress') || '',
                    streetAddress: Cookies.get('streetAddress') || '',
                    //city: getCookie('city') || '',
                    city: Cookies.get('city') || '',
                    //district: getCookie('district') || '',
                    district: Cookies.get('district') || '',
                    //postalCode: getCookie('postalCode') || '',
                    postalCode: Cookies.get('postalCode') || '',
                    latitude: Cookies.get('latitude') || '',
                    longitude: Cookies.get('longitude') || '',

                },

                //guestCount: parseInt(getCookie('guestCount')) || 1,
                guestCount: parseInt(Cookies.get('guestCount')) || 1,
                //bedrooms: parseInt(getCookie('bedrooms')) || 1,
                bedrooms: parseInt(Cookies.get('bedrooms')) || 1,
                //beds: parseInt(getCookie('beds')) || 1,
                beds: parseInt(Cookies.get('beds')) || 0,
                //bathrooms: parseInt(getCookie('bathrooms')) || 1,
                bathrooms: parseInt(Cookies.get('bathrooms')) || 0,

                amenities: [], // 存儲選中的設施和服務的ID或名稱


                imageUrls: [],
                //roomName: getCookie('roomName') || '',// 初始化房源名稱
                roomName: Cookies.get('roomName') || '',// 初始化房源名稱
                //roomDescription: getCookie('roomDescription') || '',
                roomDescription: Cookies.get('roomDescription') || '',
                //roomPrice: parseFloat(getCookie('roomPrice')) || 100, // 初始值100或cookie中的值
                roomPrice: parseFloat(Cookies.get('roomPrice')) || 100, // 初始值100或cookie中的值

            }, // 用於儲存每個 container 的資料
            // Include other data properties as needed

            progressBar: {
                ProgressOnePercentage: 0,
                ProgressTwoPercentage: 0,
                ProgressThreePercentage: 0

            },

        };

    },
    methods: {

        goToNextContainer() {

            if (!this.isNextButtonDisabled) {
                this.currentContainer++;
                this.calculateProgress();
                this.saveFormData();// Save when moving to the next container
            }
        },
        goToPreviousContainer() {
            //防止currentContainer低於1
            if (this.currentContainer > 1) {
                this.currentContainer--;
                this.calculateProgress();
            }
        },
        calculateProgress() {
            // Progress calculation logic here
            if (this.currentContainer >= 1 && this.currentContainer <= 5) {
                // 第一階段: 每個 container 增加 8%，總共 5 個 container，所以最大為 48%
                this.progressBar.ProgressOnePercentage = (this.currentContainer - 1) * (40 / 6);
                this.progressBar.ProgressTwoPercentage = 0;
                this.progressBar.ProgressThreePercentage = 0;
            } else if (this.currentContainer >= 7 && this.currentContainer <= 10) {
                // 第二階段: 固定第一階段為 40%，第二階段每個 container 增加 10%，總共 5 個 container，所以最大為 40%
                this.progressBar.ProgressOnePercentage = 40;
                this.progressBar.ProgressTwoPercentage = (this.currentContainer - 5) * (40 / 5);
                this.progressBar.ProgressThreePercentage = 0;
            } else if (this.currentContainer >= 12 && this.currentContainer <= 13) {
                // 第三階段: 固定第一和第二階段分別為 40% 和 40%，第三階段每個 container 增加 20%，總共 3 個 container，所以最大為 20%
                this.progressBar.ProgressOnePercentage = 40;
                this.progressBar.ProgressTwoPercentage = 40;
                this.progressBar.ProgressThreePercentage = (this.currentContainer - 10) * (20 / 3);
            }
        },
        async initMap() {
            const { Map, Marker } = await google.maps.importLibrary("maps");
            this.map = new Map(this.$refs.mapContainer, {
                center: { lat: 23.49047998956327, lng: 120.23842372870054 },
                zoom: 12,
            });
            this.geocoder = new google.maps.Geocoder();
        },
        debouncedUpdateMap: _.debounce(function () {
            this.updateMap();
        }, 1300),
        updateMap() {
            const address = `${this.formData.address.postalCode} ${this.formData.address.country} ${this.formData.address.city} ${this.formData.address.district} ${this.formData.address.streetAddress} ${this.formData.address.latitude} ${this.formData.address.longitude}`;
            this.geocoder.geocode({ 'address': address }, (results, status) => {
                if (status === 'OK') {
                    const location = results[0].geometry.location;
                    this.formData.address.latitude = location.lat().toString();
                    this.formData.address.longitude = location.lng().toString();
                    this.map.setCenter(location);
                    if (this.marker) {
                        this.marker.setMap(null);
                    }
                    this.marker = new google.maps.Marker({
                        map: this.map,
                        position: location,
                    });
                    this.saveFormData(); // 保存更新的 formData
                } else {
                    console.error('Geocode was not successful for the following reason:', status);
                }
            });
        },


        loadFormData() {

            //const formDataJson = getCookie('formData');
            const formDataJson = Cookies.get('formData');
            if (formDataJson) {
                this.formData = JSON.parse(formDataJson);
                console.log("FormData loaded", this.formData);
            }
        },
        saveFormData() {

            console.log("Saving formData to cookie", this.formData);
            const formDataJson = JSON.stringify(this.formData);
            //setCookie('formData', formDataJson, 2)// Save for 2 days
            Cookies.set('formData', formDataJson, { expires: 2 })// Save for 2 days
            console.log("FormData saved", this.formData);
        },
        async submitForm() {
            try {
                // Fetch the CSRF token from the meta tag
                const token = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
                // Updated submitData structure to match backend expectations
                const submitData = {
                    //memberId: parseInt(this.formData.memberId, 10),
                    memberId: this.formData.memberId,
                    roomCategory: this.formData.roomCategory,
                    address: this.formData.address,
                    guestCount: this.formData.guestCount,
                    bedrooms: this.formData.bedrooms,
                    beds: this.formData.beds,
                    bathrooms: this.formData.bathrooms,
                    amenities: this.formData.amenities,
                    imageUrls: this.formData.imageUrls,
                    roomName: this.formData.roomName.trim(),
                    roomDescription: this.formData.roomDescription.trim(),
                    roomPrice: this.formData.roomPrice,

                    // Include other formData fields as needed...
                };
                // Show a loading dialog using SweetAlert
                Swal.fire({
                    title: '處理中...',
                    text: '請稍後...',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
                // Send the POST request to the server with CSRF token in the headers
                //https://localhost:7166/Hosting/CreateRoomApi
                ///api/HostingApi/CreateRoomApi
                const response = await fetch('/api/HostingApi/CreateRoomApi', {//fetch 函數返回一個 Promise，該 Promise 解析為一個 Response 物件，代表了網路請求的結果。這個請求是發送到 /api/HostingApi/CreateRoomApi 這個 URL。
                    method: 'POST',
                    // mode: 'cors',
                    // cache: 'no-cache',
                    // credentials: 'same-origin',
                    headers: {
                        'Content-Type': 'application/json',
                        // Include CSRF token in the request headers
                        'RequestVerificationToken': token,//這是一個安全性檢查，用於防止跨站點偽造請求（CSRF）攻擊。token 變數應該包含一個有效的 CSRF 驗證令牌，這個令牌通常是在用戶登入時生成的，並且在每個請求中都需要包含。
                        //'X-CSRF-TOKEN': document.head.querySelector('meta[name="csrf-token"]').content// Include CSRF token,Vue inside a Blade template
                        //'X-CSRF-TOKEN': token, // 確保這行存在且 token 是正確的
                    },
                    body: JSON.stringify(submitData),//將 JavaScript 物件轉換為 JSON 純文字
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                // Process the response from the server
                const data = await response.json();// 假設後端return ok成功、重定導向URL和memberName
                // console.log('Success:', data);
                // // Upon successful creation, remove the formData cookie
                // Cookies.remove('formData');
                // // Redirect to the room source page or display a success message
                // window.location.href = '/Hosting/Source';
                if (data.success) {
                    // If the operation was successful, redirect the user
                    this.memberName = data.memberName; // 確保這樣設置 memberName
                    Swal.fire({
                        title: '成功!',
                        text: '您已成功上傳您的房源。',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            //Cookies.remove('formData');
                            window.location.href = data.redirectUrl;
                        }
                    });
                } else {
                    // Handle cases where the server indicates failure
                    Swal.fire('運行失敗', '發生錯誤。 請再試一次。', '錯誤');
                }
            }

            catch (error) {
                console.error('Error:', error);
                // Handle errors here, perhaps display a message to the user
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: '有些問題!',
                    footer: '請稍後再試。'
                });
            }
        },

        //add new method for Selection and Updates for id
        selectRoomCategory(categoryValue) {
            // Assuming `categoryValue` is the value attribute of the radio input
            this.formData.roomCategory = this.roomCategoryMap[categoryValue];
            this.saveFormData(); // Save updated formData
        },
        //add new method for Selection and Updates for id
        updateAmenities(event) {
            const checked = event.target.checked;
            const amenityKey = event.target.id; // 'id' corresponds to keys in `amenityMap`
            const amenityId = this.amenityMap[amenityKey];

            if (checked && !this.formData.amenities.includes(amenityId)) {
                this.formData.amenities.push(amenityId);
            } else if (!checked) {
                const index = this.formData.amenities.indexOf(amenityId);
                if (index > -1) {
                    this.formData.amenities.splice(index, 1);
                }
            }

            this.saveFormData(); // Save updated formData
        },

        increaseCount(type) {
            if (this.formData[type] < 99) { // 假設99是上限
                this.formData[type]++;
                this.saveFormData(); // 保存更新後的formData到cookie
            }
        },
        decreaseCount(type) {
            if (this.formData[type] > 1) { // 最小值為1
                this.formData[type]--;
                this.saveFormData(); // 保存更新後的formData到cookie
            }
        },

        initializeCheckboxStates() {
            this.formData.amenities.forEach((amenityId) => {
                const checkbox = document.getElementById(amenityId);
                if (checkbox) {
                    checkbox.checked = true;
                }
            });
        },

        handleFileSelect(event) {
            const files = event.target.files;
            if (!files.length) return;

            //Add sweetalert
            //  檢查文件數量是否滿足要求
            if (files.length < 7) {
                Swal.fire({
                    icon: 'error',
                    title: '錯誤',
                    text: '至少需要上傳7張圖片',
                    confirmButtonText: '確定',
                });
                return;
            }
            // 上傳前提示
            Swal.fire({
                title: '圖片上傳中...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                },
            });

            this.formData.imageUrls = []; // 清空先前的圖片URLs

            //sweetalert
            let uploadedCount = 0; // 用於跟蹤上傳完成的圖片數量

            for (let file of files) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    // 直接將圖片DataURL新增到imageUrls陣列，用於顯示
                    this.formData.imageUrls.push(e.target.result);


                    //sweetalert
                    // 每讀取完成一個文件，就視為一個文件上傳完成
                    uploadedCount++;
                    if (uploadedCount === files.length) {
                        // 所有文件都已處理完畢，開始上傳邏輯
                        this.uploadImages(files).then(urls => {
                            if (urls && urls.length > 0) {
                                this.formData.imageUrls = urls; // 更新formData中的imageUrls
                                this.$nextTick(() => {
                                    this.$refs.hiddenInput.value = this.formData.imageUrls.join(',');
                                });
                                this.saveFormData(); // 保存formData到Cookie

                                // 上傳成功提示
                                Swal.fire({
                                    icon: 'success',
                                    title: '上傳成功',
                                    showConfirmButton: false,
                                    timer: 1500,
                                });
                            }
                        }).catch(error => {
                            console.error('上傳失敗:', error);
                            Swal.fire({
                                icon: 'error',
                                title: '上傳失敗',
                                text: error.toString(),
                            });
                        });
                    }
                };

                reader.readAsDataURL(file);
            }
        },

        async uploadImages(files) {
            const formData = new FormData();
            for (let i = 0; i < files.length; i++) {
                formData.append('files', files[i]); // 'files' 是後端接收文件的參數名
            }

            try {
                const response = await fetch('/api/UploadImage/upload', { // 修改為我們的上傳 API 端點
                    method: 'POST',
                    body: formData,
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const urls = await response.json(); // 假設後端返回的是上傳圖片的 URL 陣列
                this.formData.imageUrls = urls;// 更新formData中的imageUrls
                // // 更新Cookie
                // setCookie('imageUrls', JSON.stringify(urls), 2); // 保存2天
                return urls;
            } catch (error) {
                console.error('Error:', error);
                //return []; // 發生錯誤時返回空陣列
                throw error; //  拋出錯誤，以便調用方處理
            }
        },

        increasePrice() {
            if (this.formData.roomPrice < 99999) { // max
                this.formData.roomPrice++;
                this.saveFormData();
            }
        },
        decreasePrice() {
            if (this.formData.roomPrice > 100) { // min
                this.formData.roomPrice--;
                this.saveFormData();
            }
        },
        initialRoomCategory() {
            const entry = Object.entries(this.roomCategoryMap).find((category) => category[1] === this.formData.roomCategory)
            console.log(entry);
            if (entry) {
                this.selectedRoomCategory = entry[0];
            }
            //if (this.formData) {
            //    console.log(Object.entries(this.roomCategoryMap))
            //    const entry = Object.entries(this.roomCategoryMap).find((category) => category[1] === this.formData.roomCategory)
            //    console.log(entry);
            
            //    //this.selectedRoomCategory = entry[0];

            //}
          
        }

    },
    computed: {
        roomNameLength() {
            // 使用可選鏈操作符安全訪問length屬性
            return this.formData.roomName?.length || 0;
        },

        roomDescriptionLength() {
            // 使用可選鏈操作符安全訪問length屬性
            return this.formData.roomDescription?.length || 0;
        },
        // fullAddress(){
        //     //return `${this.formData.address.streetAddress}, ${this.formData.address.district}, ${this.formData.address.city}, ${this.formData.address.country}, ${this.formData.address.postalCode}`;//combine user input address fields info.
        //     return `${this.formData.address.postalCode}${this.formData.address.country}${this.formData.address.city}${this.formData.address.district}${this.formData.address.streetAddress}`;//combine user input address fields info.
        // },
        setRoomCategoryFinished() {
            return this.formData.roomCategory > 0;
        },
        setRoomAddressFinished() {
            return this.formData.address.country.length > 0
                && this.formData.address.city.length > 0
                && this.formData.address.streetAddress.length > 0
                && this.formData.address.district.length > 0
                && this.formData.address.postalCode.length > 0;
        },
        setRoomCountsFinished() {
            return this.formData.guestCount > 0
                && this.formData.bedrooms > 0
                && this.formData.beds >= 0
                && this.formData.bathrooms >= 0;
        },
        setfacilityServiceFinished() {
            return this.formData.amenities.length >= 1;//用戶只要選擇了至少一個設施或服務，按鈕就會是可用狀態。
        },
        setRoomImagesFinished() {
            return this.formData.imageUrls.length > 0;
        },
        setRoomNameFinished() {
            return this.formData.roomName.length > 0;
        },
        setRoomDescriptionFinished() {
            return this.formData.roomDescription.length > 0;
        },
        setRoomPriceFinished() {
            return this.formData.roomPrice > 100;
        },

    },
    watch: {
        // 'formData.roomCategory'(newVal) {
        //  localStorage.setItem('roomCategory', newVal);

        //cookie
        'formData.roomCategory': function (newVal) {
            this.saveFormData();// Save to cookie on change// Corrected path
        },
        'formData.roomName': function (newVal, oldVal) {
            if (newVal !== oldVal) {
                this.saveFormData();
            }
        },
        'formData.roomDescription': function (newVal, oldVal) {
            if (newVal !== oldVal) {
                this.saveFormData();
            }
        },
        'formData.roomPrice': function (newVal, oldVal) {
            if (newVal !== oldVal) {//每當用戶更改房價輸入框中的值時，這個值就會被保存到cookie中，並且在頁面加載時通過loadFormData方法從cookie中加載出來。
                this.saveFormData();
            }
        },
    },
    mounted() {
        //console.log(this.formData);
        //this.formData.memberId = globalMemberId; // Set MemberId here
        this.initMap();
        this.calculateProgress();
        this.loadFormData();// Load saved form data when the component is mounted // Ensure this correctly initializes formData from cookies
        //this.initMap();
        this.initialRoomCategory();
        this.$nextTick(() => {
            // 確保DOM已更新
            this.initializeCheckboxStates(); // 根據加載的formData初始化復選框的狀態

        });
        //console.log("MemberId set to: ", this.formData.memberId);// 初始化其他 formData 屬性

    },
}).mount('#app');