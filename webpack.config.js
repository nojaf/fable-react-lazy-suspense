// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

const babel = {};

module.exports = {
  mode: "development",
  entry: "./src/App.fsx",
  output: {
    path: path.join(__dirname, "./docs"),
    filename: "[name].bundle.js",
    chunkFilename: "[name].bundle.js",
    publicPath: "/"
  },
  devServer: {
    contentBase: "./public",
    port: 8080,
    historyApiFallback: true
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: {
            babel
          }
        }
      }
    ]
  },
  optimization: {
    splitChunks: {
      chunks: "all"
    }
  },
  plugins: [
    new HtmlWebpackPlugin({
      filename: "index.html",
      template: "./public/index.html"
    })
  ]
};
