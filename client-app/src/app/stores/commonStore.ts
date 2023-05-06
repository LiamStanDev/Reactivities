import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore {
  error: ServerError | null = null;
  token: string | null = localStorage.getItem("jwt"); // if no => null
  appLoaded = false;

  constructor() {
    makeAutoObservable(this);

    // mobx's reaction only react event after the observable change
    reaction(
      () => this.token,
      (token) => {
        if (token) {
          // if token has been set, set local storage
          localStorage.setItem("jwt", token);
        } else {
          // if token is null, set local storage
          localStorage.removeItem("jwt");
        }
      },
    );
  }

  setServerError = (error: ServerError) => {
    this.error = error;
  };

  setToken = (token: string | null) => {
    // update the state in react
    this.token = token;
  };

  setAppLoaded = () => {
    this.appLoaded = true;
  };
}
