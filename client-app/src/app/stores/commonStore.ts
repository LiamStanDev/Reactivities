import { makeAutoObservable } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore {
  error: ServerError | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  setServerError(error: ServerError) {
    this.error = error;
  }
}
