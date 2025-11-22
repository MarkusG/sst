import { PropsWithChildren } from "react";

interface DraggableProps<T> {
  data: T;
  onDragStart: (e: React.DragEvent<HTMLDivElement>, data: T) => Promise<void>;
  onDragEnd: (e: React.DragEvent<HTMLDivElement>, data: T) => Promise<void>;
}

export default function Draggable<T>(
  props: PropsWithChildren<DraggableProps<T>>,
) {
  async function onDragStart(e: React.DragEvent<HTMLDivElement>) {
    e.dataTransfer?.setData("application/json", JSON.stringify(props.data));
    await props.onDragStart(e, props.data);
  }

  async function onDragEnd(e: React.DragEvent<HTMLDivElement>) {
    e.preventDefault();
    const data = JSON.parse(e.dataTransfer.getData("application/json")) as T;
    await props.onDragEnd(e, data);
  }

  return (
    <div draggable onDragStart={onDragStart} onDragEnd={onDragEnd}>
      {props.children}
    </div>
  );
}
