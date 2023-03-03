import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link, useParams } from "react-router-dom";
import { Card, Image, ButtonGroup, Button } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";

export default observer(function ActivityDetails() {
  const { activityStore } = useStore();
  const { selectedActivity: activity, loadActivity } = activityStore; // alias
  const { id } = useParams();

  useEffect(() => {
    if (id) loadActivity(id);
  }, [id, loadActivity]);

  if (!activity) return <LoadingComponent />;
  return (
    <Card fluid>
      <Image src={`/assets/categoryImages/${activity.category}.jpg`} /* In React, "/" means "public" folder. */ />
      <Card.Content>
        <Card.Header>{activity.title}</Card.Header>
        <Card.Meta>
          <span>{activity.date}</span>
        </Card.Meta>
        <Card.Description>{activity.description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <ButtonGroup widths="2">
          <Button as={Link} to={`/manage/${activity.id}`} basic color="blue" content="Edit" />
          <Button as={Link} to="/activities" basic color="grey" content="Cancel" />
        </ButtonGroup>
      </Card.Content>
    </Card>
  );
});
