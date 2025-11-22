import { useContext } from "react";
import { DefaultImportContext, ImportContext } from "./ImportContext.tsx";
import { Link } from "react-router-dom";

export default function Done() {
  const [context, setContext] = useContext(ImportContext);

  if (!context.done) return;

  return (
    <div className="w-full h-full flex flex-col justify-center items-center">
      <div className="text-green-500 flex flex-col items-center gap-2">
        <div className="flex flex-col items-center">
          <i className="fa fa-4x fa-check"></i>
          <p className="text-2xl mb-2">Done!</p>
        </div>
        <Link to="/transactions">
          <div className="bg-green-100 rounded px-2">
            <span className="mr-2">Go to transactions</span>
            <i className="fa fa-arrow-right"></i>
          </div>
        </Link>
        <button onClick={() => setContext(DefaultImportContext)}>
          <div className="bg-gray-100 text-gray-700 rounded px-2">
            <span className="mr-2">Import again</span>
            <i className="fa fa-rotate-right"></i>
          </div>
        </button>
      </div>
    </div>
  );
}
