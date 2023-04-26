import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { Activity } from "../models/activity";
import { router } from "../router/Routes";

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.defaults.baseURL = "http://localhost:5000/api";

// interceptor: 攔截器
// 只會攔截response, request不會攔截
axios.interceptors.response.use(
  async (respone) => {
    await sleep(1000);
    return respone;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
      case 400:
        if (data.errors) {
          const modalStateErrors = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modalStateErrors.push(data.errors[key]);
            }
          }
          throw modalStateErrors.flat();
        } else {
          toast.error(data);
        }
        toast.error("bad request");
        break;
      case 401:
        toast.error("unauthorised");
        break;
      case 403:
        toast.error("forbiddne");
        break;
      case 404:
        router.navigate("/not-found");
        // toast.error("not found");
        break;
      case 500:
        toast.error("server error");
        break;
    }
    return Promise.reject(error);
  },
);

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
