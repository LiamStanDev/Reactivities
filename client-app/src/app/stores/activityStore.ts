import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Activity } from "../models/activity";
import { v4 as uuid } from "uuid";
import { format } from "date-fns";
export default class ActivityStore {
  activityRegistry = new Map<string, Activity>();
  selectedActivity: Activity | undefined = undefined; // don't use null, alwayse use undefine.
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    // makeObservable(this, {
    //   title: observable,
    //   setTitle: action,
    // });
    makeAutoObservable(this);
  }
  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort((a, b) => {
      // Comparator
      return a.date!.getTime() - b.date!.getTime(); // the swap condition
    });
  }

  get groupedActivities() {
    // (accumulate, current)
    const groupedByDate = this.activitiesByDate.reduce(
      (groupedByDate, activity) => {
        const date = format(activity.date!, "dd MMMM yyyy"); // as a key
        // if this date in the activities map, then add current activity into the map value list,
        // if not, then create a list with the current activity
        // 因為get方法的返回值是Activity | undefine, 所以使用類型斷言as or !
        // 使用as就算真的為null編譯器不會拋出異常，！會拋出異常
        groupedByDate.has(date)
          ? groupedByDate.set(date, [
              ...(groupedByDate.get(date) as Activity[]),
              activity,
            ])
          : groupedByDate.set(date, [activity]);
        return groupedByDate;
      },
      new Map<string, Activity[]>(),
    ); // the second parameter is the "initail value" of accumulate
    return groupedByDate;
  }

  loadActivites = async () => {
    this.setLoadingInitial(true);
    try {
      const activities = await agent.Activities.list();
      // the following codes are in different ticks
      activities.forEach((activity) => {
        this.setActivity(activity);
      });
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };
  /**
   * because loadActivities is an async function which after await
   * the cpu tick is different, but MobX in strict mode warning that
   * change observable need to use action. As a result, I create this
   * action to deal with this warning.
   */
  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  /**
   * 重要概念：使用await一定有回返值，返回值一定是Promise，就算沒有return也是一樣
   * 但是如果還有return的話那Promise中的範行就會被return指定，如下面的function
   * 返回值是Promise<Activity | undefined>
   */
  loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.selectedActivity = activity;
      return activity;
    } else {
      this.setLoadingInitial(true);
      try {
        activity = await agent.Activities.details(id);
        this.setActivity(activity);
        runInAction(() => {
          this.selectedActivity = activity;
        });
        this.setLoadingInitial(false);
        return activity;
      } catch (error) {
        console.log(error);
        this.setLoadingInitial(false);
        return undefined;
      }
    }
  };
  private setActivity = (activity: Activity) => {
    activity.date = new Date(activity.date!);
    this.activityRegistry.set(activity.id, activity);
  };
  private getActivity = (id: string) => {
    return this.activityRegistry.get(id);
  };

  selectActivity = (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
  };

  cancelSelectedActivity = () => {
    this.selectedActivity = undefined;
  };

  openForm = (id?: string) => {
    id ? this.selectActivity(id) : this.cancelSelectedActivity();
    this.editMode = true;
  };

  closeForm = () => {
    this.editMode = false;
  };

  createActivity = async (activity: Activity) => {
    this.loading = true;
    activity.id = uuid();
    try {
      await agent.Activities.create(activity);
      runInAction(() => {
        // use runInAction for after await code that change state in MobX strict mode.
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  updateActivity = async (activity: Activity) => {
    this.loading = true;
    try {
      await agent.Activities.update(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteActivity = async (id: string) => {
    this.loading = true;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        this.activityRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
