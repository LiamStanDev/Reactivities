import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";

export default class UserStore {
  user: User | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  get isLoggIn() {
    return !!this.user; // if user is undefined, return false: !! will cast to boolean
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      console.log(user);
    } catch (error) {
      throw error;
    }
  };
}
