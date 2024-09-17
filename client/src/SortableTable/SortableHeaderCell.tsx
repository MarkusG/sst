import { PropsWithChildren, useContext } from "react";
import { TableContext } from "./SortableTable";

interface SortableHeaderCellProps {
    className?: string,
    field: string
}

function SortableHeaderCell(props: PropsWithChildren<SortableHeaderCellProps>) {
    const ctx = useContext(TableContext);

    function cycleDirection() {
        if (ctx?.options.field !== props.field) {
            ctx?.setOptions({ field: props.field, direction: "up" });
            return;
        }

        switch (ctx?.options.direction) {
            case null:
                ctx?.setOptions({ field: props.field, direction: "up" });
                break;
            case "up":
                ctx?.setOptions({ field: props.field, direction: "down" });
                break;
            case "down":
                ctx?.setOptions({});
                break;
        }
    }

    return (
        <td className={props.className}>
            <button className="w-full pr-1 flex justify-between items-baseline" onClick={cycleDirection}>
                {props.children}
                <div className="w-4">
                    {ctx?.options.field === props.field && ctx?.options.direction === "up" && <i className="fa-solid fa-sort-up"></i>}
                    {ctx?.options.field !== props.field && <i className="fa-solid fa-sort text-gray-400"></i>}
                    {ctx?.options.field === props.field && ctx?.options.direction === "down" && <i className="fa-solid fa-sort-down"></i>}
                </div>
            </button>
        </td>
    );
}

export default SortableHeaderCell;
