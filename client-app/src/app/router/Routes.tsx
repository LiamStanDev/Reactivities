import { createBrowserRouter, Navigate, RouteObject } from "react-router-dom"; // from web router
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import ActivityDetails from "../../features/activities/details/ActivityDetails";
import ActivityForm from "../../features/activities/form/ActivityForm";
import NotFound from "../../features/error/NotFound";
import ServerError from "../../features/error/ServerError";
import TestError from "../../features/error/TestError";
import App from "../layout/App";

const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "activities", element: <ActivityDashboard /> },
      { path: "activities/:id", element: <ActivityDetails /> },
      {
        /* 因為React router為了節省渲染的資源，所以若Router到相同的Component並不會重新渲染，但是createActivityForm應該內容要清空
    所以我們給Router component一個key讓react render知道，這兩個是不同的，要重新渲染 */
      },
      { path: "createActivity", element: <ActivityForm key="create" /> },
      { path: "manage/:id", element: <ActivityForm key="manage" /> },
      { path: "errors", element: <TestError /> },
      { path: "not-found", element: <NotFound /> },
      { path: "server-error", element: <ServerError /> },
      { path: "*", element: <Navigate replace to="/not-found" /> },
    ],
  },
];
export const router = createBrowserRouter(routes);
