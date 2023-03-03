import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import ActivityFilters from "./ActivityFilter";
import ActivityList from "./ActivityList";
// import 的順序非常重要 ：can't access lexical declaration '__WEBPACK_DEFAULT_EXPORT__' before initialization
// 因為ActivityDetails 必須在ActivityList與ActivityForm之後，不然按鈕會指不到ActivityDeails

export default observer(function ActivityDashboard() {
  const { activityStore } = useStore();
  const { loadActivites, activityRegistry } = activityStore;

  useEffect(() => {
    if (activityRegistry.size <= 1) loadActivites();
  }, [activityRegistry.size, loadActivites]); // only when activityStore change the useEffect will be call again.

  if (activityStore.loadingInitial) return <LoadingComponent content="Loading app" />;

  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList />
      </Grid.Column>
      <Grid.Column width="6">
        <ActivityFilters />
      </Grid.Column>
    </Grid>
  );
});
