import { observer } from "mobx-react-lite";
import { ChangeEvent, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { Button, Form, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Activity } from "../../../app/models/activity";
import { useStore } from "../../../app/stores/store";
import { v4 as uuid } from "uuid";

export default observer(function ActivityForm() {
  const { activityStore } = useStore();
  const { createActivity, updateActivity, loading, loadActivity, loadingInitial } = activityStore;
  // get id from React Router
  const { id } = useParams();
  const navigate = useNavigate();

  const [activity, setActivity] = useState<Activity>({
    id: "",
    title: "",
    category: "",
    description: "",
    date: "",
    city: "",
    venue: "",
  });

  useEffect(() => {
    if (id) loadActivity(id).then(activity => setActivity(activity!)); // we giveup to deal with the type of the input param of setActivity to undefine, using !
  }, [id, loadActivity]); // because when id change, we need to load different form.

  function handleSubmit() {
    if (!activity.id) {
      activity.id = uuid();
      createActivity(activity).then(() => navigate(`/activities/${activity.id}`));
    } else {
      updateActivity(activity).then(() => navigate(`/activities/${activity.id}`));
    }
  }

  function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    /* the event type can be seen by hover on the onChange element */
    const { name, value } = event.target; // event.target is html dom can get the name and value.
    setActivity({ ...activity, [name]: value }); // 這個方法需要對下面表單所有選項都適用，本來是要寫Title, Description...每個都需要建立一個方法去更新指定的屬性，但這樣子就能使用一個方法，首先去獲得html中的name的值，作為物件屬性的名稱，並使用value將其進行賦值，這樣子的寫法非常適合用於表單操作。
  }

  if (loadingInitial) return <LoadingComponent content="Loading activity..." />;

  return (
    <Segment clearing>
      <Form onSubmit={handleSubmit} autoComplete="off">
        <Form.Input placeholder="Title" value={activity.title} name="title" onChange={handleInputChange} />
        <Form.TextArea
          placeholder="Description"
          value={activity.description}
          name="description"
          onChange={handleInputChange}
        />
        <Form.Input placeholder="Category" value={activity.category} name="category" onChange={handleInputChange} />
        <Form.Input type="date" placeholder="Date" value={activity.date} name="date" onChange={handleInputChange} />
        <Form.Input placeholder="City" value={activity.city} name="city" onChange={handleInputChange} />
        <Form.Input placeholder="Venue" value={activity.venue} name="venue" onChange={handleInputChange} />
        <Button loading={loading} floated="right" positive type="submit" content="Submit" />{" "}
        {/* many of component of semantic ui has loading indicator */}
        <Button as={Link} to="/activities" floated="right" type="button" content="Cancel" />
      </Form>
    </Segment>
  );
});
