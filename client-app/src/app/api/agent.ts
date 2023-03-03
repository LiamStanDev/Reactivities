import axios, { AxiosResponse } from "axios";
import { Activity } from "../models/activity";

const sleep = (delay: number) => {
  return new Promise(resolve => {
    setTimeout(resolve, delay);
  });
};

axios.defaults.baseURL = "http://localhost:5000/api";

// interceptor: 攔截器
// 只會攔截response, request不會攔截
axios.interceptors.response.use(async respone => {
  try {
    await sleep(1000);
    return respone;
  } catch (error) {
    console.log(error);
    return await Promise.reject(error);
  }
});

// use generic method to make the code reusable, because the response body may be Activity, Activity[].
// the first <T> : set the generic method with generic type call "T".
// AxiosResponse is a generic Interface.
const resonseBody = <T>(response: AxiosResponse<T>): T => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(resonseBody),
  post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(resonseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(resonseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(resonseBody),
};

// contain our all activities
const Activities = {
  list: () => requests.get<Activity[]>("/activities"),
  details: (id: string) => requests.get<Activity>(`/activities/${id}`),
  create: (activity: Activity) => requests.post<void>("/activities", activity), // we dot need the respone body information, set void.
  update: (activity: Activity) => requests.put<void>(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.del<void>(`/activities/${id}`),
};

const agent = {
  Activities,
};

export default agent;
