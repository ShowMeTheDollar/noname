const LOGIN_PAGE_URL = '/auth/login'
const handle401Unauthorized = (response) => {
    window.location.href = LOGIN_PAGE_URL
}

const useRequest = function () {
    // NOTE: 請根據後端API位置調整
    const BASE_URL = '/'
    const request = axios.create({ baseURL: BASE_URL }) //axios有封裝東西，如fetch, xml..., $Ajax...；長出一個axios的物件

    const beforeRequest = (config) => {
        // 發 request 前處理

        // 如果有 JWT Token 就帶
        const token = localStorage.getItem('token')
        token && (config.headers['Authorization'] = `Bearer ${token}`)
        //如驗證方式用jwt，token放在header上的Authorization上；只要透過這裡就加header上去

        return config
    }

    // 請求攔截器
    request.interceptors.request.use(beforeRequest)//request的攔截器；成功的callback

    const responseSuccess = (response) => {
        // 2XX
        // NOTE: 請根據後端API接口做調整

        // console.log(response)
        //return response.data
        if (response.data.isSuccess) {
            return response.data.body
        }
        Promise.reject(response.data.message)//確保流程上的錯誤會吐我想傳的訊息
    }

    const responseFail = (err) => {
        // !2XX

        // console.log(err)
        const { response } = err
        const { statusText, status } = response

        // NOTE: 統一處理失敗行為
        switch (status) {
            case 401:
                // TODO: handle 401 ex. redirect
                handle401Unauthorized(response)//狀態是401就倒去登入頁
                break
        }

        return Promise.reject(response)
    }

    // 回應攔截器
    request.interceptors.response.use(responseSuccess, responseFail)//攔截器對於...失敗的callback


    return {
        httpGet: request.get,
        httpPost: request.post,
        httpPut: request.put,
        httpDelete: request.delete,
    }//常用的CRUD
}

const {
    httpPost,
    httpGet,
    httpDelete,
    httpPut,
} = useRequest();//直接寫在windows上，這四個封裝好的
//這樣沒寫就是var，會放在window上

//httpPost = useRequest().httpPost