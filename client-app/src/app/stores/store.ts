import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";

interface Store {
  activityStore: ActivityStore;
}

export const store: Store = {
  activityStore: new ActivityStore(),
};

export const StoreContext = createContext(store);

/**
 * make a function that we can use useStore instead of
 * using useContext(StoreContext).
 */
export function useStore() {
  return useContext(StoreContext); // a react Hook
}
