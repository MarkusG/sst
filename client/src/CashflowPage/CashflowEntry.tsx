import { useState } from "react";
import Amount from "../Amount";
import { CategoryCashFlowResponse } from "../Contracts/Responses";

export interface CashflowEntryProps {
  category: CategoryCashFlowResponse;
  level: number;
  hidden: boolean;
}

export default function CashflowEntry({
  category,
  level,
  hidden,
}: CashflowEntryProps) {
  const [open, setOpen] = useState(true);
  return (
    <>
      <div
        className={`${hidden ? "hidden" : "contents"} [&_div]:even:bg-gray-100`}
      >
        <div
          className="text-left bg-inherit whitespace-nowrap overflow-hidden text-ellipsis"
          style={{ paddingLeft: `calc(.5rem + ${level * 0.5}rem)` }}
        >
          {category.subcategories.length > 0 && (
            <button onClick={() => setOpen(!open)}>
              <span>{category.name}</span>
              <i
                className={`ml-1 fa fa-xs fa-chevron-${open ? "down" : "right"}`}
              ></i>
            </button>
          )}
          {category.subcategories.length === 0 && <span>{category.name}</span>}
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[0] : category.treeTotals[0]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[1] : category.treeTotals[1]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[2] : category.treeTotals[2]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[3] : category.treeTotals[3]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[4] : category.treeTotals[4]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[5] : category.treeTotals[5]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[6] : category.treeTotals[6]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[7] : category.treeTotals[7]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[8] : category.treeTotals[8]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={open ? category.categoryTotals[9] : category.treeTotals[9]}
          />
        </div>
        <div className="px-2">
          <Amount
            amount={
              open ? category.categoryTotals[10] : category.treeTotals[10]
            }
          />
        </div>
        <div className="px-2">
          <Amount
            amount={
              open ? category.categoryTotals[11] : category.treeTotals[11]
            }
          />
        </div>
        <div className="px-2 border-l border-gray-300">
          <Amount
            amount={open ? category.yearCategoryTotal : category.yearTreeTotal}
          />
        </div>
      </div>
      {category.subcategories.map((c) => (
        <CashflowEntry
          key={c.id}
          category={c}
          level={level + 1}
          hidden={hidden || !open}
        />
      ))}
    </>
  );
}
