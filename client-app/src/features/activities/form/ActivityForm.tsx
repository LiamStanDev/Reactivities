import { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";

interface Props {
  activity: Activity | undefined;
  closeForm: () => void;
  createOrEdit: (activity: Activity) => void;
}

export default function ActivityForm({ activity: selectedActivity /* renaming */, closeForm, createOrEdit }: Props) {
  const initialState = selectedActivity ?? {
    /* means thant selectedActivity is unidentified use the following like C# ??= */ id: "",
    title: "",
    category: "",
    description: "",
    date: "",
    city: "",
    venue: "",
  };

  const [activity, setActivity] = useState(initialState);

  function handleSubmit() {
    createOrEdit(activity);
    closeForm();
  }

  function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    /* the event type can be seen by hover on the onChange element */
    const { name, value } = event.target; // event.target is html dom can get the name and value.
    setActivity({ ...activity, [name]: value }); // 這個方法需要對下面表單所有選項都適用，本來是要寫Title, Description...每個都需要建立一個方法去更新指定的屬性，但這樣子就能使用一個方法，首先去獲得html中的name的值，作為物件屬性的名稱，並使用value將其進行賦值，這樣子的寫法非常適合用於表單操作。
  }

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
        <Form.Input placeholder="Date" value={activity.date} name="date" onChange={handleInputChange} />
        <Form.Input placeholder="City" value={activity.city} name="city" onChange={handleInputChange} />
        <Form.Input placeholder="Venue" value={activity.venue} name="venue" onChange={handleInputChange} />
        <Button floated="right" positive type="submit" content="Submit" />
        <Button onClick={closeForm} floated="right" type="button" content="Cancel" />
      </Form>
    </Segment>
  );
}
