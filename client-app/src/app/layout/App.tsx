import { useEffect, useState } from "react";
import axios from "axios"; // axios use typescript so we can use intellisece
import { Container } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid"; // because it's not a typescript, you need to check the hover

function App() {
  const [activities, setActivities] = useState<Activity[]>([]); // useState by adding specific type
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    axios.get<Activity[]>("http://localhost:5000/api/activities").then(response => {
      setActivities(response.data);
    });
  }, []);

  function handleSelectedActivity(id: string) {
    setSelectedActivity(activities.find(x => x.id === id));
  }

  function handleCancelSelectActivity() {
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string) {
    // because the Create Activity Button not input id, so use id?
    id ? handleSelectedActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }
  /*
   * id?: string vs id: string | undefined
   * 第一種為id這個參數可有可無，第二種是id可以傳一個未定義，所以第二個參數一定要有。
   */

  function handleFormClose() {
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Activity) {
    activity.id
      ? setActivities([...activities.filter(x => x.id !== activity.id), activity]) // 先把不是的activity從activities中解構出來，後面添加更新的activity。
      : setActivities([...activities, { ...activity, id: uuid() }]); // 新的就直接添加
  }

  function handleDeleteActivity(id: string) {
    setActivities([...activities.filter(x => x.id !== id)]);
  }

  return (
    <>
      <NavBar openForm={handleFormOpen} />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashboard
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectedActivity}
          cancelSelectedActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
        />
      </Container>
    </>
  );
}

export default App;
