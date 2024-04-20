import axios, { AxiosResponse } from "axios";

const env = import.meta.env;
axios.defaults.baseURL = env.VITE_BACKEND_BASE_URL;
const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use(config => {
const user = localStorage.getItem('user');
if(user !== null){
    if(config.headers){
      config.headers.Authorization = `Bearer ${JSON.parse(user).token}`
    }
  }
  return config;
}

);

axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      localStorage.clear();
    }
    return Promise.reject(error);
  }
);

export const request = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body:any) =>axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body:any) =>axios.put<T>(url, body).then(responseBody),
  patch: <T>(url: string, body:any) =>axios.patch<T>(url, body).then(responseBody),
}