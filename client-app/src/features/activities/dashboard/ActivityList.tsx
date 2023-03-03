import { observer } from "mobx-react-lite";
import { Fragment } from "react";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import ActivityListItem from "./ActivityListItem";

export default observer(function ActivityList() {
  const { activityStore } = useStore();
  const { groupedActivities } = activityStore;

  return (
    <>
      {Array.from(groupedActivities).map(([date, activities]) => {
        return (
          // 要先將map轉換成Array才能使用map函數
          <Fragment key={date}>
            <Header sub color="teal">
              {date}
            </Header>

            {activities.map(activity => (
              <ActivityListItem key={activity.id} activity={activity} />
            ))}
          </Fragment>
        );
      })}
    </>
  );
});
