import React, { useEffect, useState } from "react";
import logo from "./logo.svg";
import "./App.css";
import axios from "axios"; // axios use typescript so we can use intellisece
import { Button, Header, List } from "semantic-ui-react";

function App() {
  const [activies, setActivities] = useState([]);

  useEffect(() => {
    axios.get("http://localhost:5000/api/activities").then(response => {
      setActivities(response.data);
    });
  }, []);

  return (
    <div>
      <Header as="h2" icon="users" content="Reactivities" />
        <List>
          {activies.map((activity: any) => {
            return (
              <List.Item key={activity.id}>
                {activity.title}
              </List.Item>
            )
          })}
      </List>
      <Button content="test" />
    </div>
  );
}

export default App;
